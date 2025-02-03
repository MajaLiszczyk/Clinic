import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { passwordValidator } from '../validators';
import { Registrant } from '../model/registrant';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
   registrants: Registrant[] = [];
    registrantForm: FormGroup;
    isDisable = false;
    registrant: Registrant;
    isEditableMode = false;
    isAddingMode = false; //niepotrzebne?
  
    constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
      this.registrantForm = this.formBuilder.group({});
      this.registrant = { id: 0, name: '', surname: '', registrantNumber: '', isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    }
  
    ngOnInit() {
      this.getAllRegistrants();
      this.registrantForm = this.formBuilder.group({
        id: Number,
        name: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        surname: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        registrantNumber: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[0-9]{5}$/)] }),
        email: new FormControl(null, {
          validators: [Validators.required, Validators.email, // Sprawdza poprawność adresu email
          Validators.maxLength(256)]
        }),
        password: new FormControl(null, { validators: [Validators.required, passwordValidator] }),
      });
    }
  
    get formId(): FormControl { return this.registrantForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formName(): FormControl { return this.registrantForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formSurname(): FormControl { return this.registrantForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formRegistrantNumber(): FormControl { return this.registrantForm?.get("registrantNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formEmail(): FormControl { return this.registrantForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formPassword(): FormControl { return this.registrantForm?.get("password") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  
    setConditionalValidation() {
      const emailC = this.registrantForm.get('email');
      const passwordC = this.registrantForm.get('password');
  
      if (this.isAddingMode) {
        emailC?.setValidators([Validators.required]);
        passwordC?.setValidators([Validators.required]);
  
      } else {
        // Usuń walidator `required`
        emailC?.clearValidators();
        passwordC?.clearValidators();
      }
      // Uruchom ponowną walidację
      emailC?.updateValueAndValidity();
      passwordC?.updateValueAndValidity();
    }
  
    getAllRegistrants() {
      this.clinicService.getAllRegistrants().subscribe(data => {
        this.registrants = data;
      })
    }
  
    edit(registrant: Registrant) {
        this.formId.setValue(registrant.id);
        this.formName.setValue(registrant.name);
        this.formSurname.setValue(registrant.surname);
        this.formRegistrantNumber.setValue(registrant.registrantNumber);
        this.isEditableMode = true;
        this.isAddingMode = false; //niepotrzebne?
        this.setConditionalValidation();
      }
    
      addNewRegistrant() {
        this.isAddingMode = true;
        this.isEditableMode = false;
        this.setConditionalValidation();
        console.log('isAddingMode in AddLaboratoryWorker:', this.isAddingMode);
      }
    
      cancelAdding() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.registrantForm.reset();
      }
    
      cancelEditing() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.registrantForm.reset();
      }
  
       createRegistrantAccount() {
          if (this.registrantForm.invalid) {
            this.registrantForm.markAllAsTouched();
            return;
          }
          this.clinicService.createRegistrantAccount(this.registrantForm.getRawValue()) // Bez obiektu opakowującego
            .subscribe({
              next: (result: Registrant) => {
                this.registrant = result; // Zwrócony obiekt przypisany do zmiennej
                this.getAllRegistrants();
                this.isAddingMode = false;
                this.registrantForm.reset();
              },
              error: (err) => {
                console.error("Error occurred:", err); // Obsługa błędów
              }
            });
        }
  
        update(){
          if(this.registrantForm.invalid){
            this.registrantForm.markAllAsTouched(); 
            return;
          }
          const laboratoryWorkerData = this.registrantForm.getRawValue();
          this.clinicService.updateLaboratoryWorker(laboratoryWorkerData)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAllRegistrants();
              this.isEditableMode = false;
              this.registrantForm.reset();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          })
        }
  
        /*archive(registrantId: number){
          this.clinicService.archiveRegistrant(registrantId)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAllRegistrants();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          });
        } */

}
