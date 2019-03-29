using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace OnBoardLMS.Web
{
    public class Validator
    {
	    //todo: make this arg a generic
	    #region valid
	    public static bool Valid(object[] ToTest)
        {
            return Valid(ToTest, typeof(System.String));
        }
        public static bool Valid(object[] ToTest, Type TargetType)
        {
            foreach(object val in ToTest)
            if (!Valid(val, TargetType)) return false;
            return true;
        }
		/// <summary>
		/// Validates the length of a string
		/// </summary>
		/// <param name="ToTest">The string to test</param>
		/// <param name="min">The minimum length of the string</param>
		/// <param name="max">The maximum length of the string</param>
		/// <returns>True if string has valid length, false if not.</returns>
		public static bool ValidLength(string ToTest, int min, int max)
		{
			if (!Valid(ToTest,typeof(string))) return false;
			if (ToTest.Length < min) return false;
			if (ToTest.Length > max) return false;
			return true;
		}
    public static bool Valid(object ToTest)
    {
      return Valid(ToTest, typeof(System.String));
    }
    public static bool Valid(object ToTest, Type TargetType)
    {
        string val = ToTest != null ? ToTest.ToString() : "";
        switch (TargetType.FullName)
        {
            case "System.Int32":
            {
            int outval = -1;
            return Int32.TryParse(val, out outval);            
            }
				case "System.String":
            {
            return ToTest != null && val.Length > 0;
            }
            case "System.DateTime":
            {
            DateTime outval;
            return ToTest != null && DateTime.TryParse(val, out outval);
            }
            case "System.Guid":
            {
            return new System.Text.RegularExpressions.Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$").Match(ToTest!=null?ToTest.ToString():string.Empty).Success;
            }
		    case "System.Boolean":
			{
				//accept 1,0
				int temp = GetInt(ToTest, 0, 1, -1);
				if (temp != -1) return true; //!=-1 means it was 1 or 0
				//accept regular bools
				bool tempBool;
				return bool.TryParse(val, out tempBool);
			}
		    case "System.Int64":
			{
				long outval = -1;
				return Int64.TryParse(val, out outval);
			}
		    case "OnBoardLMS.CommaDelimitedInt":
			{
				return ToTest!=null && System.Text.RegularExpressions.Regex.Match(ToTest.ToString(), @"^(\d+\,?)+$").Success;
			}
		    case "OnBoardLMS.SecureFormInput":
			{
				//validate for string (blank and null)
				if (!Valid(ToTest, typeof(string))) return false;

				string value = ToTest.ToString();

				//todo: allow chars only (0-9, a-z, A-Z,@,.,-) (regEx)
						
				//validate for spaces
				//if (value.Contains(" ")) return false;//fixed for users with spaces in name chavez jr1234

				//validate for special chars (%,--)
				//if (value.Contains("%") || value.Contains("--") || value.Contains("'")) return false;
						
				//todo: validate for sql key words												
				return true;
			}
        default: return false;
        }
      //return ToTest != null && ToTest.Length > 0;      
		}

		//[AM 11.30.11] added new sig 
		public static bool ValidateEmail(string Email)
		{
			return ValidateEmail(Email, false);
		}

		//[AM 11.30.11] Change sig to include new arg
		public static bool ValidateEmail(string Email, bool ExcludeNoReply)
        {
			if (ExcludeNoReply)
			{
				if (Email == null || System.Text.RegularExpressions.Regex.Match(Email, @"no.{0,1}reply.*").Success) return false;
			}
			return System.Text.RegularExpressions.Regex.IsMatch(Email, @"^([\w-\'\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,10}|[0-9]{1,10})(\]?)$");
		}

		public static bool IsNoReplyEmail(string Email)
		{
			if (Email != null)
				return System.Text.RegularExpressions.Regex.Match(Email, @"no.{0,1}reply.*").Success;
			else
				return false;
		}
		/// <summary>
		/// This will use the LUHN validation to check whether the card number given was valid
		/// If any character is not a digit this will return false
		/// found algorithm at http://www.superstarcoders.com/blogs/posts/luhn-validation-for-asp-net-web-forms-and-mvc.aspx
		/// </summary>
		/// <param name="cardNumber">string of numbers that make up card</param>
		/// <returns>true if valid false if invalid card number</returns>
		public static bool IsValidCreditCardNumber(string cardNumber)
		{
			//validate card number. 
			if (Validator.GetString(cardNumber) == null) throw new ArgumentException("card number");

			//remove illegal chars
			cardNumber = cardNumber.Replace(" ", "").Replace("-","");
			
			if (cardNumber.Any(c => !Char.IsDigit(c)))
			{
				return false;
			}
			int checksum = cardNumber
				 .Select((number, index) => (number - '0') << ((cardNumber.Length - index - 1) & 1))
				 .Sum(num => num > 9 ? num - 9 : num);

			return (checksum % 10) == 0 && checksum > 0;

		}

		/// <summary>
		/// Validates password 
		/// </summary>
		/// <param name="password">password string to be validated</param>
		/// <returns>true if valid false if invalid password</returns>
		public static bool IsValidPassword(string password)
		{
			int
				numeric = new Regex(@"[\d]").IsMatch(password) ? 1 : 0,
				uppercase = new Regex(@"[A-Z]").IsMatch(password) ? 1 : 0,
				lowercase = new Regex(@"[a-z]").IsMatch(password) ? 1 : 0,
				special = new Regex(@"[\W_]").IsMatch(password) ? 1 : 0,
				size = new Regex(@".{8,64}$").IsMatch(password) ? 1 : 0,
				illegal = new Regex(@"(?:[&\|\f\n\r\t\v])").IsMatch(password) ? 0 : 1;
			if ((numeric + uppercase + lowercase + special) > 2 && size == 1 && illegal == 1)
				return true;
			else
			{
				StringBuilder shouldContain = new StringBuilder("Password does not meet at least one of the following the requirements:");
				shouldContain.Append(numeric == 0 ? "<br/>numbers" : "");
				shouldContain.Append(uppercase == 0 ? "<br/>uppercase letters" : "");
				shouldContain.Append(lowercase == 0 ? "<br/>lowercase letters" : "");
				shouldContain.Append(special == 0 ? "<br/>special characters" : "");
				shouldContain.Append(size == 0 ? "<br/>at least 8 characters long" : "");
				shouldContain.Append(illegal == 0 ? "<br/>contains illegal characters" : "");
				throw new Exception(shouldContain.ToString());
			}

		}
		#endregion

		#region Exists
		//used to verify that a URL parameter exists (not null)
		//example: Request.QueryString["arg"]==null means it doesn't exist (was never provided in URL)
		public static bool Exists(object raw)
		{
			return raw != null;
		} 
		#endregion		

		#region Get Values
		public static int GetBit(object value)
		{
			return GetInt(value,0,1,-1);
		}
		public static int GetInt(object value)
		{
			return GetInt(value, 0, Int32.MaxValue, -1);
		}
		public static int GetInt(object value, int? minimum, int? maximum)
		{
			return GetInt(value, minimum, maximum, -1);
		}
		public static int GetInt(object value, int? minimum, int? maximum, int defaultValue)
    {
			//validate
			if (!Valid(value, typeof(int))) return defaultValue;
			
			//get valid int to work with
			int newval = Convert.ToInt32(value);

			//enforce min/max
			if (minimum == null && maximum != null) if (newval > maximum) return defaultValue;
			if (maximum == null && minimum != null) if (newval < minimum) return defaultValue;
			if (maximum != null && minimum != null) if (newval < minimum || newval > maximum) return defaultValue;

			//all clear return new int
			return newval;
    }

		#region GetLong
		public static long GetLong(object value)
		{
			return GetLong(value, 0, Int64.MaxValue, -1);
		}
		public static long GetLong(object value, long? minimum, long? maximum)
		{
			return GetLong(value, minimum, maximum, -1);
		}
		public static long GetLong(object value, long? minimum, long? maximum, long defaultValue)
		{
			//validate
			if (!Valid(value, typeof(long))) return defaultValue;

			//get valid int to work with
			long newval = Convert.ToInt64(value);

			//enforce min/max
			if (minimum == null && maximum != null) if (newval > maximum) return defaultValue;
			if (maximum == null && minimum != null) if (newval < minimum) return defaultValue;
			if (maximum != null && minimum != null) if (newval < minimum || newval > maximum) return defaultValue;

			//all clear return new int
			return newval;
		} 
		#endregion


		//validates a string and returns null if value is blank or null
		public static string GetString(object value)
		{
			return GetString(value, true);
		}

        //Gets base64 encoded string and converts to string. If null, returns blank string
        public static string GetBase64String(String value)
        {
            var val = Convert.FromBase64String(value);
            var newVal = "";
            if(val != null)
            {
                newVal = Encoding.UTF8.GetString(val);
            }
            else
            {
                newVal = "";
            }
            newVal = WebUtility.UrlDecode(newVal);
            return GetString (newVal,true);
        }

		//validates a string and returns null if value is blank or null
		public static string GetString(object value, bool Scrub)
		{
			//todo: scrub for SQL injection attacks as well.
			//return Valid(value, typeof(string)) ? UISafeString.Protect(value.ToString()).Trim() : null;
            return "";
		}

		//validates a datetime value and returns DateTime.MinValue if value is blank or null or invalid
		public static DateTime GetDateTime(object value)
		{
			//validate
			if (!Valid(value, typeof(DateTime))) return DateTime.MinValue;

			return DateTime.Parse(value.ToString());
		}

		//get boolean
		public static bool GetBoolean(object value)
		{
			if (!Valid(value, typeof(Boolean))) return false;

			//accept 1,0
			int temp = GetInt(value, 0, 1, -1);
			if (temp != -1) return temp == 1;

			//regular strings
			return bool.Parse(value.ToString());
		}

		public static bool? GetNullableBoolean(object value)
		{
			if (!Valid(value, typeof(Boolean))) return null;

			//accept 1,0
			int temp = GetInt(value, 0, 1, -1);
			if (temp != -1) return temp == 1;

			//regular strings
			return bool.Parse(value.ToString());
		}

		//get guid
		public static Guid GetGuid(object value)
		{
			//validate
			if (!Valid(value, typeof(Guid))) return Guid.Empty;

			return new Guid(value.ToString());
		}

		//get double
		public static double GetDouble(object value)
		{
			double result = -1d;
			if (value != null && double.TryParse(value.ToString(), out result)) return result;
			return -1d;
		}
		#endregion

		#region Get comma delimited values
		//get comma delimited string
		public static string GetCommaDelimitedInt(object value)
		{
			//validate
			//if (!Valid(value, typeof(OnBoardLMS.CommaDelimitedInt))) return null;

			//trim trailing commas
			return value.ToString().Trim(new char[] { ' ', ',' });
		}

		//get comma delimited string
		public static string GetCommaDelimitedString(object value)
		{
			//validate
			if (value == null) return null;
			if (!Valid(value.ToString().Split(','), typeof(System.String))) return null;

			//trim trailing commas
			return value.ToString().Trim(new char[] { ' ', ',' });
		}

		//get comma delimited guid
		public static string GetCommaDelimitedGuid(object value)
		{
			//validate
			if (value == null) return null;
			if (!Valid(value.ToString().Split(','), typeof(System.Guid))) return null;

			//trim trailing commas
			return value.ToString().Trim(new char[] { ' ', ',' });
		} 
		#endregion

		
	}
}