import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { LaboratoryWorker } from '../model/laboratory-worker';
import { ClinicService } from '../services/clinic.service';
import { passwordValidator } from '../validators';

@Component({
  selector: 'app-registrant-laboratory-workers',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './registrant-laboratory-workers.component.html',
  styleUrl: './registrant-laboratory-workers.component.css'
})
export class RegistrantLaboratoryWorkersComponent {
  laboratoryWorkers: LaboratoryWorker[] = [];
  laboratoryWorkerForm: FormGroup;
  isDisable = false;
  laboratoryWorker: LaboratoryWorker;
  isEditableMode = false;
  isAddingMode = false; //niepotrzebne?
  registrantId: number = 0;

  constructor(private route: ActivatedRoute, private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.laboratoryWorkerForm = this.formBuilder.group({});
    this.laboratoryWorker = { id: 0, name: '', surname: '', laboratoryWorkerNumber: '', isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId']; // Przypisanie id z URL
      });
    this.getAllLaboratoryWorkers();
    this.laboratoryWorkerForm = this.formBuilder.group({
      id: Number,
      name: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
      surname: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
      laboratoryWorkerNumber: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[0-9]{5}$/)] }),
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
  get formLaboratoryWorkerNumber(): FormControl { return this.laboratoryWorkerForm?.get("laboratoryWorkerNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
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
    this.clinicService.getAllLaboratoryWorkers().subscribe(data => {
      this.laboratoryWorkers = data;
    })
  }

  edit(laboratoryWorker: LaboratoryWorker) {
      this.formId.setValue(laboratoryWorker.id);
      this.formName.setValue(laboratoryWorker.name);
      this.formSurname.setValue(laboratoryWorker.surname);
      this.formLaboratoryWorkerNumber.setValue(laboratoryWorker.laboratoryWorkerNumber);
      this.isEditableMode = true;
      this.isAddingMode = false; //niepotrzebne?
      this.setConditionalValidation();
    }
  
    addNewLaboratoryWorker() {
      this.isAddingMode = true;
      this.isEditableMode = false;
      this.setConditionalValidation();
      console.log('isAddingMode in AddLaboratoryWorker:', this.isAddingMode);
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
        this.clinicService.createLaboratoryWorkerAccount(this.laboratoryWorkerForm.getRawValue()) // Bez obiektu opakowującego
          .subscribe({
            next: (result: LaboratoryWorker) => {
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
        this.clinicService.updateLaboratoryWorker(laboratoryWorkerData)
        .subscribe({
          next: (response) => {
            console.log("Action performed successfully:", response);
            this,this.getAllLaboratoryWorkers();
            this.isEditableMode = false;
            this.laboratoryWorkerForm.reset();
          },
          error: (error) => {
            console.error("Error performing action:", error);
          }
        })
      }

      archive(laboratoryWorkerId: number){
        this.clinicService.archiveLaboratoryWorker(laboratoryWorkerId)
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
