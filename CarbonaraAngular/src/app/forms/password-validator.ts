import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

const PASSWORD_VALIDATOR_MINLENGTH = 8;

export function carbonaraPasswordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        
        return checkPassword(control.value) ? { passwordInvalid: true} : null;
    }
}

export function checkPassword(value: string) : PasswordErrors | null
{
    if (!value) {
        return new PasswordErrors(true, true, true, true, true);
    }

    const hasUpper = /[A-Z]+/.test(value);
    const hasLower = /[a-z]+/.test(value);
    const hasNumber = /[0-9]+/.test(value);
    const hasOtherChars = /[\w]+/.test(value);
    const isMeetingMinLength = value.length >= PASSWORD_VALIDATOR_MINLENGTH;

    const isValid = hasUpper && hasLower && hasNumber && hasOtherChars && isMeetingMinLength;

    const errors : PasswordErrors = new PasswordErrors(!hasUpper, !hasNumber, !hasOtherChars, !isMeetingMinLength, !hasLower);

    return isValid ? null : errors;
}

export class PasswordErrors {

    private errors: string[] = [];
    public errorString: string = "";

    public constructor(
        public passwordUpperInvalid: boolean,
        public passwordNumberInvalid: boolean,
        public passwordOthersInvalid: boolean,
        public passwordLengthInvalid: boolean,
        public passwordLowerInvalid: boolean
    )
    {
        this.addToErrorString(passwordLengthInvalid, "Die Mindestlänge von " + PASSWORD_VALIDATOR_MINLENGTH + " Zeichen wurde nicht erreicht");
        this.addToErrorString(passwordUpperInvalid, "Es sind keine Großbuchstaben vorhanden");
        this.addToErrorString(passwordLowerInvalid, "Es sind keine Kleinbuchstaben vorhanden");
        this.addToErrorString(passwordNumberInvalid, "Es sind keine Ziffern vorhanden");
        this.addToErrorString(passwordOthersInvalid, "Es sind keine Sonderzeichen vorhanden");

        for (let i = 0; i < this.errors.length; i++)
        {
            let conjunction = i > 0 ? (i == this.errors.length - 1 ? " und " : ", ") : ""; 
            this.errorString += conjunction + this.errors[i];
        }
    }

    private addToErrorString(value: boolean, errorText: string)
    {
        if (!value)
            return;

        if (this.errors.length > 0)
        {
            this.errors.push(errorText[0].toLowerCase() + errorText.substring(1));
        } else this.errors.push(errorText);
    }

    public static fromErrors(errors: PasswordErrors) : PasswordErrors
    {
        return new PasswordErrors(errors.passwordUpperInvalid, errors.passwordNumberInvalid, errors.passwordOthersInvalid, errors.passwordLengthInvalid, errors.passwordLowerInvalid);
    }

}