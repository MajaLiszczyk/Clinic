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


export function passwordValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.value;
  const errors: any = {};

  if (!/\d/.test(password)) {
    errors.hasNumber = 'Password must contain at least one digit.';
  }
  if (!/[A-Z]/.test(password)) {
    errors.hasUppercase = 'Password must contain at least one uppercase letter.';
  }
  if (!/[a-z]/.test(password)) {
    errors.hasLowercase = 'Password must contain at least one lowercase letter.';
  }
  if (!/[\W]/.test(password)) {
    errors.hasSpecialCharacter = 'Password must contain at least one special character.';
  }
  if (!password || password.length < 6 || password.length > 100) {
    errors.isValidLength = 'Password must be between 6 and 100 characters long.';
  }

  return Object.keys(errors).length ? errors : null; // Zwracaj błędy tylko, jeśli istnieją
}


/*export function passwordValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.value;
  const hasNumber = /\d/.test(password); // Czy zawiera cyfrę
  const hasUppercase = /[A-Z]/.test(password); // Czy zawiera wielką literę
  const hasLowercase = /[a-z]/.test(password); // Czy zawiera małą literę
  const hasSpecialCharacter = /[\W]/.test(password); // Czy zawiera znak specjalny
  const isValidLength = password?.length >= 6 && password?.length <= 100; // Czy długość jest odpowiednia

  if (hasNumber && hasUppercase && hasLowercase && hasSpecialCharacter && isValidLength) {
    return null; // Walidacja poprawna
  }

  return { passwordInvalid: true }; // Walidacja niepoprawna
}*/