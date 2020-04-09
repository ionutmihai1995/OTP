import { Injectable } from '@angular/core';
import { FormGroup } from "@angular/forms";

@Injectable()
export class FormHelperService {
    public showErrorMessage(form: FormGroup, controlName: string, validator: string): boolean {
        if (controlName === '') { return false; };
        if(!form)
            return false;
        let control = form.get(controlName);
        if ((control || null) === null) { return false; };
        
        return validator === ''
            ? control.invalid && control.touched
            : control.hasError(validator) && control.touched;
    }
}