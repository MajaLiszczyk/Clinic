import { AbstractControl, ValidationErrors, ValidatorFn, FormArray } from '@angular/forms';

export function atLeastOneSelectedValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formArray = control as FormArray;
    const isAtLeastOneChecked = formArray.controls.some(control => control.value);
    return isAtLeastOneChecked ? null : { required: true };
  };
}