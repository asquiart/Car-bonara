import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const NUMBER_INVALID_ERROR = { notANumber: true };

export function ValidateNumber() : ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null =>  {
        let stringValue = (control.value.toString() as string);
        let value = +stringValue;
        let containsSpaces = stringValue.includes(' ');
        return (isNaN(value) || containsSpaces) ? NUMBER_INVALID_ERROR : null;
    }
}