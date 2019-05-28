/* eslint-disable */
import validator from 'validator';
import { formatDate, parseDate } from "react-day-picker/moment";

class FormValidator {
  constructor(validations) {
    // validations is an array of validation rules specific to a form
    this.validations = validations;
  }

  validate(state) {
    // start out assuming valid
    let validation = this.valid();

    // for each validation rule
    this.validations.forEach(rule => {

      let position = rule.field.split("+")[0]; // valueSelected

      // if the field hasn't already been marked invalid by an earlier rule
      if (!validation[position].isInvalid) {
        // determine the field value, the method to invoke and optional args from 
        // the rule definition
        // const field_value = state[position] ? state[position].valueSelected.toString() : "";
        const field_value = state[position] ? (typeof state[position].valueSelected === 'object' ? formatDate(state[position].valueSelected.from || "", 'L', 'en') + " and " + formatDate(state[position].valueSelected.to || "", 'L', 'en') : state[position].valueSelected.toString()) : "";
        const args = rule.args || [];
        const validation_method =
          typeof rule.method === 'string' ?
            validator[rule.method] :
            rule.method

        const dataType = state[position] ? state[position].type : "=";
        const operator = state[position] ? state[position].operatorsSelected.value : "=";

        // call the validation_method with the current field value as the first
        // argument, any additional arguments, and the whole state as a final
        // argument.  If the result doesn't match the rule.validWhen property,
        // then modify the validation object for the field and set the isValid
        // field to 
        if (!rule.isSkipValidation) {
          if (validation_method(field_value, ...args, state) !== rule.validWhen) {
            validation[position] = { isInvalid: true, message: rule.message }
            validation.isValid = false;
          }

          if (this.isEmptyOrSpaces(field_value)) {
            validation[position] = { isInvalid: true, message: rule.message }
            validation.isValid = false;
          }

          if (dataType == "date" && operator == "Between") {
            let isValid = false,
                message = "";
            let date_range = state[position] ? (typeof state[position].valueSelected === 'object' ? formatDate(state[position].valueSelected.from || "", 'L', 'en') + " and " + formatDate(state[position].valueSelected.to || "", 'L', 'en') : state[position].valueSelected.toString()) : "";
            if(date_range != ""){
              date_range = date_range.split('and');
              if (date_range[0].trim() == "Invalid date" && date_range[1].trim() == "Invalid date") {
                isValid = false;
                validation.isValid = isValid;
                message = rule.message + "|both";
              } else if (date_range[0].trim() == "Invalid date") {
                isValid = false;
                validation.isValid = isValid;
                message = rule.message + "|from";
              }  else if (date_range[1].trim() == "Invalid date") {
                isValid = false;
                validation.isValid = isValid;
                message = rule.message + "|to";
              } else {
                isValid = true;
                message = "";
              }
            } else {
              isValid = false;
              validation.isValid = isValid;
              message = rule.message + "|both";
            }
  
            validation[position] = { isInvalid: !isValid, message: message }
          }

        } else {
          validation[position] = { isInvalid: false, message: '' }
          validation.isValid = true;
        }

      }
    });

    return validation;
  }

  valid() {
    const validation = {}
    this.validations.map(rule => (
      validation[rule.field.split("+")[0]] = { isInvalid: false, message: '' }
    ));

    return { isValid: true, ...validation };
  }

  isEmptyOrSpaces(str) {
    let value = str || "";
    return value === null || value.match(/^ *$/) !== null;
  }
}

export default FormValidator;