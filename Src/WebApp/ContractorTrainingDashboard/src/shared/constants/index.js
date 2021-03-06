/* eslint-disable */
/* Constants for API Requests */

const getAPIEndpoint = async () => {
    const data = await fetch('config/appsettings.json').then(response =>  response.json()
    ).then((response) => {
        return response.Apigateway.Endpoint + response.Apigateway.Stagename  || "";
    });
    return data;
}
export { getAPIEndpoint };

export const API_CONFIG = {};

/* Endpoint API for Sysvine Dev */
export const API_DOMAIN = 'https://44x7tie9i9.execute-api.us-west-2.amazonaws.com/';

/* Endpoint API for ITS Dev */
// export const API_DOMAIN = 'https://cvyf80gu2f.execute-api.us-west-2.amazonaws.com/';

export const API_STAGE_NAME = 'dev';

/* Constants for Validations */
export const FIELD_ERROR_MESSAGE = 'This field is required';
export const QUERY_BULIDER_GUIDANCE_MESSAGE = 'Please customize and run your query in the above window. The search result(s) will be displayed here.';
export const RESULT_SET_EMPTY_MESSAGE = 'Sorry, no records';

/* Constants for Workbook Dashboard */
export const GET_COMPLETED_WORKBOOKS_COLUMNS = ["USER_ID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE", "LAST_ATTEMPT_DATE"];
export const GET_ASSIGNED_WORKBOOKS_COLUMNS = ["USER_ID", "WORKBOOK_ID", "ROLE", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_TASK", "TOTAL_TASK", "DUE_DATE"];
export const GET_EMPLOYEES_COLUMNS = ["USER_ID", "SUPERVISOR_ID", "EMPLOYEE_NAME", "ROLE", "ASSIGNED_WORKBOOK", "WORKBOOK_DUE", "PAST_DUE_WORKBOOK", "COMPLETED_WORKBOOK", "TOTAL_EMPLOYEES"];
export const GET_WORKBOOKS_PROGRESS_COLUMNS = ["USER_ID", "WORKBOOK_ID", "TASK_ID", "TASK_CODE", "TASK_NAME", "TOTAL_TASK",  "INCOMPLETE_TASK", "COMPLETED_TASK"];
export const GET_WORKBOOKS_PAST_DUE_COLUMNS = ["USER_ID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE"];
export const GET_WORKBOOKS_COMING_DUE_COLUMNS = ["USER_ID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE"];
export const GET_WORKBOOKS_REPETITION_COLUMNS = ["NUMBER_OF_ATTEMPTS", "STATUS", "LAST_ATTEMPT_DATE", "LOCATION", "EVALUATOR_NAME", "COMMENTS"];

/* Constants for OQ Dashboard */
export const GET_COMPANY_QUALIFICATION_COLUMNS = ["USER_ID", "COMPANY_NAME", "COMPANY_ID", "ASSIGNED_COMPANY_QUALIFICATION", "COMPLETED_COMPANY_QUALIFICATION", "IN_COMPLETE_COMPANY_QUALIFICATION", "PAST_DUE_COMPANY_QUALIFICATION", "IN_DUE_COMPANY_QUALIFICATION", "TOTAL_COMPANY_EMPLOYEES"];
export const GET_QUALIFICATION_COLUMNS = ["USER_ID", "COMPANY_ID", "EMPLOYEE_NAME","ROLE","TOTAL_EMPLOYEES","ASSIGNED_QUALIFICATION","COMPLETED_QUALIFICATION","IN_DUE_QUALIFICATION","PAST_DUE_QUALIFICATION","IN_COMPLETE_QUALIFICATION"];
export const GET_CONTRACTOR_QUALIFICATION_COLUMNS = ["USER_ID", "COMPANY_ID", "EMPLOYEE_NAME","TOTAL_EMPLOYEES","ASSIGNED_QUALIFICATION","COMPLETED_QUALIFICATION","IN_DUE_QUALIFICATION","PAST_DUE_QUALIFICATION","IN_COMPLETE_QUALIFICATION"];
export const GET_EMPLOYEE_QUALIFICATION_COLUMNS = ["USER_ID", "ROLE", "COMPANY_ID","EMPLOYEE_NAME","TOTAL_EMPLOYEES","ASSIGNED_QUALIFICATION","COMPLETED_QUALIFICATION","IN_DUE_QUALIFICATION","PAST_DUE_QUALIFICATION","IN_COMPLETE_QUALIFICATION"];
export const GET_ASSIGNED_QUALIFICATION_COLUMNS = ["TASK_CODE","TASK_NAME","EMPLOYEE_NAME","ASSIGNED_DATE"];
export const GET_COMPLETED_QUALIFICATION_COLUMNS = ["TASK_CODE","TASK_NAME","EMPLOYEE_NAME","COURSE_EXPIRATION_DATE"];
export const GET_IN_COMPLETED_QUALIFICATION_COLUMNS = ["TASK_CODE","TASK_NAME","EMPLOYEE_NAME","ASSIGNED_DATE"];
export const GET_PAST_DUE_QUALIFICATION_COLUMNS = ["TASK_CODE","TASK_NAME","EMPLOYEE_NAME","COURSE_EXPIRATION_DATE"];
export const GET_COMING_DUE_QUALIFICATION_COLUMNS = ["TASK_CODE","TASK_NAME","EMPLOYEE_NAME","COURSE_EXPIRATION_DATE"];

/** Constants for Login flow */
export const AUTH_USER_NAME = "devtester@its-training.com";
export const AUTH_PASSKEY = "Demo@2017";
export const AUTO_LOGOUT_IDLE_TIME = 20;
export const AUTO_LOGOUT_MESSAGE = "You haven't had any activity in the last little while. To protect your privacy, you will be automatically logged out in the next minute. Choose cancel below to stay logged in.";
export const NO_SESSION_MESSAGE = "Sorry, something went wrong. Please refresh the page.";