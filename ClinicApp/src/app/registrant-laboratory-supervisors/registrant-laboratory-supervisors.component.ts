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
  laboratoryWorkers: LaboratorySupervisor[] = [];
    laboratoryWorkerForm: FormGroup;
    isDisable = false;
    laboratoryWorker: LaboratorySupervisor;
    isEditableMode = false;
    isAddingMode = false; //niepotrzebne?
  
    constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
      this.laboratoryWorkerForm = this.formBuilder.group({});
      this.laboratoryWorker = { id: 0, name: '', surname: '', supervisorNumber: '', isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    }
  
    ngOnInit() {
      this.getAllLaboratoryWorkers();
      this.laboratoryWorkerForm = this.formBuilder.group({
        id: Number,
        name: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        surname: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
        supervisorNumber: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[0-9]{5}$/)] }),
        email: new FormControl(null, {
          validators: [Validators.required, Validators.email, // Sprawdza poprawność adresu email
          Validators.maxLength(256)]
        }),
        password: new FormControl(null, { validators: [Validators.required, passwordValidator] }),
      });
    }
  
    get formId(): FormControl { return this.laboratoryWorkerForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formName(): FormControl { return this.laboratoryWorkerForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formSurname(): FormControl { return this.laboratoryWorkerForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formSupervisorNumber(): FormControl { return this.laboratoryWorkerForm?.get("supervisorNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formEmail(): FormControl { return this.laboratoryWorkerForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
    get formPassword(): FormControl { return this.laboratoryWorkerForm?.get("password") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  
    setConditionalValidation() {
      const emailC = this.laboratoryWorkerForm.get('email');
      const passwordC = this.laboratoryWorkerForm.get('password');
  
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

    getAllLaboratoryWorkers() {
      this.clinicService.getAllLaboratorySupervisors().subscribe(data => {
        this.laboratoryWorkers = data;
      })
    }
  
    edit(laboratoryWorker: LaboratorySupervisor) {
        this.formId.setValue(laboratoryWorker.id);
        this.formName.setValue(laboratoryWorker.name);
        this.formSurname.setValue(laboratoryWorker.surname);
        this.formSupervisorNumber.setValue(laboratoryWorker.supervisorNumber);
        //this.formLaboratoryWorkerNumber.setValue(laboratoryWorker.supervisorNumber);
        this.isEditableMode = true;
        this.isAddingMode = false; //niepotrzebne?
        this.setConditionalValidation();
      }
    
      addNewLaboratoryWorker() {
        this.isAddingMode = true;
        this.isEditableMode = false;
        this.setConditionalValidation();
      }
    
      cancelAdding() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.laboratoryWorkerForm.reset();
      }
    
      cancelEditing() {
        this.isAddingMode = false;
        this.isEditableMode = false; //niepotrzebne?
        this.laboratoryWorkerForm.reset();
      }
  
       createLaboratoryWorkerAccount() {
          if (this.laboratoryWorkerForm.invalid) {
            this.laboratoryWorkerForm.markAllAsTouched();
            return;
          }
          this.clinicService.createLaboratorySupervisorAccount(this.laboratoryWorkerForm.getRawValue()) // Bez obiektu opakowującego
            .subscribe({
              next: (result: LaboratorySupervisor) => {
                this.laboratoryWorker = result; // Zwrócony obiekt przypisany do zmiennej
                this.getAllLaboratoryWorkers();
                this.isAddingMode = false;
                this.laboratoryWorkerForm.reset();
              },
              error: (err) => {
                console.error("Error occurred:", err); // Obsługa błędów
              }
            });
        }
  
        update(){
          if(this.laboratoryWorkerForm.invalid){
            this.laboratoryWorkerForm.markAllAsTouched(); 
            return;
          }
          const laboratoryWorkerData = this.laboratoryWorkerForm.getRawValue();
          this.clinicService.updateLaboratorySupervisor(laboratoryWorkerData)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAllLaboratoryWorkers();
              this.isEditableMode = false;
              this.laboratoryWorkerForm.reset();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          })
        }
  
        archive(laboratoryWorkerId: number){
          this.clinicService.archiveLaboratorySupervisor(laboratoryWorkerId)
          .subscribe({
            next: (response) => {
              console.log("Action performed successfully:", response);
              this.getAllLaboratoryWorkers();
            },
            error: (error) => {
              console.error("Error performing action:", error);
            }
          });
        }

}
