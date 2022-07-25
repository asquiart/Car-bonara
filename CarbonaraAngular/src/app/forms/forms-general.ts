import { AbstractControl, FormControl } from "@angular/forms";

export function createControl(value: any) : AbstractControl {
    let control = new FormControl();
    control.setValue(value);
    return control;
}