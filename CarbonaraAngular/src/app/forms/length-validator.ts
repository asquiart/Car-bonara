import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const LENGTH_INVALID_ERROR = { lengthInvalid: true };

export function ValidateLength(exactLength: number) : ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null =>  {
        let value = control.value.toString();
        return value.length == exactLength ? null : LENGTH_INVALID_ERROR;
    }
}