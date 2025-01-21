import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { LaboratoryWorker } from '../model/laboratory-worker';
import { passwordValidator } from '../validators';
import { LaboratorySupervisor } from '../model/laboratory-supervisor';

@Component({
  selector: 'app-registrant-laboratory-supervisors',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './registrant-laboratory-supervisors.component.html',
  styleUrl: './registrant-laboratory-supervisors.component.css'
})
export class RegistrantLaboratorySupervisorsComponent {
    laboratorySupervisors: LaboratorySupervisor[] = [];
    laboratorySupervisorForm: FormGroup;
    isDisable = false;
    laboratorySupervisor: LaboratorySupervisor;
    isEditableMode = false;
    isAddingMode = false; //niepotrzebne?
  
    constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
      this.laboratorySupervisorForm = this.formBuilder.group({});
      this.laboratorySupervisor = { id: 0, name: '', surname: '', laboratorySupervisorNumber: '', isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    }
  
    ngOnInit() {
      this.getAlllaboratorySupervisors();
      this.laboratorySupervisorForm = this.formBuilder.group({
        id: Number,
        name: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        surname: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        laboratorySupervisorNumber: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[0-9]{5}$/)] }),
        email: new FormControl(null, {
          validators: [Validators.required, Validators.email, // Sprawdza poprawność adresu email
          Validators.maxLength(256)]
        }),
        password: new FormControl(null, { validators: [Validators.required, passwordValidator] }),
      });
    }
  
    get formId(): FormControl { return this.laboratorySupervisorForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formName(): FormControl { return this.laboratorySupervisorForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formSurname(): FormControl { return this.laboratorySupervisorForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formLaboratorySupervisorNumber(): FormControl { return this.laboratorySupervisorForm?.get("laboratorySupervisorNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formEmail(): FormControl { return this.laboratorySupervisorForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formPassword(): FormControl { return this.laboratorySupervisorForm?.get("password") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  
    setConditionalValidation() {
      const emailC = this.laboratorySupervisorForm.get('email');
      const passwordC = this.laboratorySupervisorForm.get('password');
  
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

    getAlllaboratorySupervisors() {
      this.clinicService.getAllLaboratorySupervisors().subscribe(data => {
        this.laboratorySupervisors = data;
      })
    }
  
    edit(laboratorySupervisor: LaboratorySupervisor) {
        this.formId.setValue(laboratorySupervisor.id);
        this.formName.setValue(laboratorySupervisor.name);
        this.formSurname.setValue(laboratorySupervisor.surname);
        this.formLaboratorySupervisorNumber.setValue(laboratorySupervisor.laboratorySupervisorNumber);
        //this.formLaboratoryWorkerNumber.setValue(laboratoryWorker.supervisorNumber);
        this.isEditableMode = true;
        this.isAddingMode = false; //niepotrzebne?
        this.setConditionalValidation();
      }
    
      addNewLaboratorySupervisor() {
        this.isAddingMode = true;
        this.isEditableMode = false;
        this.setConditionalValidation();
      }
    
      cancelAdding() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.laboratorySupervisorForm.reset();
      }
    
      cancelEditing() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.laboratorySupervisorForm.reset();
      }
  
       createLaboratorySupervisorAccount() {
          if (this.laboratorySupervisorForm.invalid) {
            this.laboratorySupervisorForm.markAllAsTouched();
            return;
          }
          this.clinicService.createLaboratorySupervisorAccount(this.laboratorySupervisorForm.getRawValue()) // Bez obiektu opakowującego
            .subscribe({
              next: (result: LaboratorySupervisor) => {
                this.laboratorySupervisor = result; // Zwrócony obiekt przypisany do zmiennej
                this.getAlllaboratorySupervisors();
                this.isAddingMode = false;
                this.laboratorySupervisorForm.reset();
              },
              error: (err) => {
                console.error("Error occurred:", err); // Obsługa błędów
              }
            });
        }
  
        update(){
          if(this.laboratorySupervisorForm.invalid){
            this.laboratorySupervisorForm.markAllAsTouched(); 
            return;
          }
          const laboratorySupervisorData = this.laboratorySupervisorForm.getRawValue();
          this.clinicService.updateLaboratorySupervisor(laboratorySupervisorData)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAlllaboratorySupervisors();
              this.isEditableMode = false;
              this.laboratorySupervisorForm.reset();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          })
        }
  
        archive(laboratorySupervisorId: number){
          this.clinicService.archiveLaboratorySupervisor(laboratorySupervisorId)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAlllaboratorySupervisors();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          });
        }

}
