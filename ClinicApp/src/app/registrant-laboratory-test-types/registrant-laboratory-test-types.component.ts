import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { DiagnosticTestType } from '../model/diagnostic-test-type';
import { LaboratoryTestType } from '../model/laboratory-test-type';

@Component({
  selector: 'app-registrant-laboratory-test-types',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './registrant-laboratory-test-types.component.html',
  styleUrl: './registrant-laboratory-test-types.component.css'
})
export class RegistrantLaboratoryTestTypesComponent {
  laboratoryTestTypes: LaboratoryTestType[] = [];
  laboratoryTestTypesForm: FormGroup;
  laboratoryTestType: LaboratoryTestType = { id: 0, name: '', isAvailable: true };
  isDisable = false;
  isVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.laboratoryTestTypesForm = this.formBuilder.group({});
  }

  ngOnInit() {
    this.getAllLaboratoryTestTypes();
    this.laboratoryTestTypesForm = this.formBuilder.group({
      id: Number,
      name: new FormControl(null, { validators: [Validators.required] })
    });
  }

  get formId(): FormControl { return this.laboratoryTestTypesForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.laboratoryTestTypesForm?.get("name") as FormControl };

  getAllLaboratoryTestTypes() {
    this.clinicService.getAllLaboratoryTestTypes().subscribe(data => {
      this.laboratoryTestTypes = data;
    })
  }

  edit(testType: LaboratoryTestType) {
    this.isVisible = true;
    this.isAddingMode = false;
    this.formId.setValue(testType.id);
    this.formName.setValue(testType.name);
    this.isEditableMode = true;
  }

  update() {
    if (this.laboratoryTestTypesForm.invalid) {
      this.laboratoryTestTypesForm.markAllAsTouched();
      return;
    }
    const laboratoryTestTypeData = this.laboratoryTestTypesForm.getRawValue();
    this.clinicService.updateLAboratoryTestType(laboratoryTestTypeData)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllLaboratoryTestTypes();
          this.isEditableMode = false;
          this.laboratoryTestTypesForm.reset();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })
  }

  delete(testTypeId: number) {
    this.clinicService.transferToArchiveLaboratoryTestType(testTypeId)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllLaboratoryTestTypes();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });
  }

  addNewLaboratoryTestType() {
    this.isAddingMode = true;
    this.isEditableMode = false;
    console.log('isAddingMode in DiagnosticTestType:', this.isAddingMode);
  }

  cancelAdding() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.laboratoryTestTypesForm.reset();
  }

  cancelEditing() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.laboratoryTestTypesForm.reset();
  }

  addLaboratoryTestType() {
    if (this.laboratoryTestTypesForm.invalid) {
      this.laboratoryTestTypesForm.markAllAsTouched();
      return;
    }
    const diagnosticTestData = this.laboratoryTestTypesForm.getRawValue();
    this.clinicService.addLaboratoryTestType(diagnosticTestData) // Bez obiektu opakowującego
      .subscribe({
        next: (result: DiagnosticTestType) => {
          this.laboratoryTestType = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllLaboratoryTestTypes()
          this.isAddingMode = false;
          this.laboratoryTestTypesForm.reset();
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      });
  }

}
