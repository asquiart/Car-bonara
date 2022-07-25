import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const DATE_INVALID_ERROR = { dateInvalid: true };

export function ValidateDate() : ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null =>  {
        let value = control.value as Date;
        return value ? null : DATE_INVALID_ERROR;
    }
}