import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DiagnosticTestType } from '../model/diagnostic-test-type';
import { ClinicService } from '../services/clinic.service';

@Component({
  selector: 'app-registrant-diagnostic-test-types',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './registrant-diagnostic-test-types.component.html',
  styleUrl: './registrant-diagnostic-test-types.component.css'
})

export class RegistrantDiagnosticTestTypesComponent {
  //isAddNewSpecialisationVisible: boolean = false;
  diagnosticTestTypes: DiagnosticTestType[] = [];
  diagnosticTestTypesForm: FormGroup;
  diagnosticTestType: DiagnosticTestType = { id: 0, name: '' , isAvailable: true};
  isDisable = false;
  isVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;
  registrantId: number = 0;


  constructor(private route: ActivatedRoute, private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.diagnosticTestTypesForm = this.formBuilder.group({});
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId']; // Przypisanie id z URL
      });
    this.getAllDiagnosticTestTypes();
    this.diagnosticTestTypesForm = this.formBuilder.group({
      id: Number,
      name: new FormControl(null, { validators: [Validators.required] })
    });
  }

  get formId(): FormControl { return this.diagnosticTestTypesForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.diagnosticTestTypesForm?.get("name") as FormControl };

  getAllDiagnosticTestTypes() {
    this.clinicService.getAllDiagnosticTestTypes().subscribe(data => {
      this.diagnosticTestTypes = data;
    })
  }

  edit(testType: DiagnosticTestType) {
    this.isVisible = true;
    this.isAddingMode = false;
    this.formId.setValue(testType.id);
    this.formName.setValue(testType.name);
    this.isEditableMode = true;
  }

  update() {
    if (this.diagnosticTestTypesForm.invalid) {
      this.diagnosticTestTypesForm.markAllAsTouched();
      return;
    }
    const diagnosticTestTypeData = this.diagnosticTestTypesForm.getRawValue();
    this.clinicService.updateDiagnosticTestType(diagnosticTestTypeData)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllDiagnosticTestTypes();
        this.isEditableMode = false;
        this.diagnosticTestTypesForm.reset();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })
  }

  delete(testTypeId: number) {
    this.clinicService.deleteDiagnosticTestType(testTypeId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllDiagnosticTestTypes();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

  addNewDiagnosticTestType() {
    this.isAddingMode = true;
    this.isEditableMode = false;
    console.log('isAddingMode in DiagnosticTestType:', this.isAddingMode);
  }

  cancelAdding() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.diagnosticTestTypesForm.reset();
  }

  cancelEditing() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.diagnosticTestTypesForm.reset();
  }

  addDiagosticTestType() {
    if(this.diagnosticTestTypesForm.invalid){ 
      this.diagnosticTestTypesForm.markAllAsTouched();
      return;
    } 
    const diagnosticTestData = this.diagnosticTestTypesForm.getRawValue();
    this.clinicService.addDiagosticTestType(diagnosticTestData) // Bez obiektu opakowującego
    .subscribe({
      next: (result: DiagnosticTestType) => {
        this.diagnosticTestType = result; // Zwrócony obiekt przypisany do zmiennej
        this.getAllDiagnosticTestTypes()
        this.isAddingMode = false;
        this.diagnosticTestTypesForm.reset();
      },
      error: (err) => {
        console.error("Error occurred:", err); // Obsługa błędów
      }
    });
  }
}