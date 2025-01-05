import { AbstractControl, ValidationErrors, ValidatorFn, FormArray } from '@angular/forms';

/*sprawdza, czy co najmniej jedna kontrolka w tablicy (FormArray) ma wartość true. 
Jeśli żadna nie jest zaznaczona, zwraca błąd { required: true }*/
export function atLeastOneSelectedValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formArray = control as FormArray;
    const isAtLeastOneChecked = formArray.controls.some(control => control.value);
    return isAtLeastOneChecked ? null : { required: true };
  };
}