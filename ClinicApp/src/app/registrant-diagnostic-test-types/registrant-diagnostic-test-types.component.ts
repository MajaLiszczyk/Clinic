import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
//import { AddSpecialisationComponent } from '../add-specialisation/add-specialisation.component';
//import { GetSpecialisationsComponent } from '../get-specialisations/get-specialisations.component';
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
  isAddNewSpecialisationVisible: boolean = false;
  //readonly APIUrl = "https://localhost:5001/api/DiagnosticTestType";
  diagnosticTestTypes: DiagnosticTestType[] = [];
  //specialisationForm: FormGroup;
  diagnosticTestTypesForm: FormGroup;
  diagnosticTestType: DiagnosticTestType = { id: 0, name: '' , isAvailable: true};
  isDisable = false;
  isVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.diagnosticTestTypesForm = this.formBuilder.group({});
  }

  ngOnInit() {
    this.getAllDiagnosticTestTypes();
    this.diagnosticTestTypesForm = this.formBuilder.group({
      id: Number,
      //id: new  FormControl(0, {validators: [Validators.required]}),
      name: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(50), Validators.required] })
    });
  }


  get formId(): FormControl { return this.diagnosticTestTypesForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.diagnosticTestTypesForm?.get("name") as FormControl };

  getAllDiagnosticTestTypes() {
    this.clinicService.getAllDiagnosticTestTypes().subscribe(data => {
      this.diagnosticTestTypes = data;
    })
    /*this.http.get<DiagnosticTestType[]>(this.APIUrl + "/Get").subscribe(data => {
      this.diagnosticTestTypes = data;
    }) */
  }

  edit(testType: DiagnosticTestType) {
    this.isVisible = true;
    this.isAddingMode = false; //niepotrzebne?

    /*this.specialisationForm.reset();
    this.editableMeasure = measure;
    this.isFormVisible = true;
    this.editableMode = true;
    this.operationResult = null;*/
    this.formId.setValue(testType.id);
    this.formName.setValue(testType.name);
    this.isEditableMode = true;

    //this.fillForm(measure);
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
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })
    /*this.http.put<DiagnosticTestType>(this.APIUrl + "/update", this.diagnosticTestTypesForm.getRawValue())
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      }) */
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
    /*this.http.delete<string>(this.APIUrl + "/Delete/" + testTypeId)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllDiagnosticTestTypes();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      }); */
  }

  addNewDiagnosticTestType() {
    //this.isFormVisible = true;
    this.isAddingMode = true;
    this.isEditableMode = false;
    //this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in DiagnosticTestType:', this.isAddingMode);

  }


  cancelAdding() {
    //this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  cancelEditing() {
    //this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
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
      },
      error: (err) => {
        console.error("Error occurred:", err); // Obsługa błędów
      }
    });
    /*this.http.post<DiagnosticTestType>(this.APIUrl + "/create", this.diagnosticTestTypesForm.getRawValue()) // Bez obiektu opakowującego
      .subscribe({
        next: (result: DiagnosticTestType) => {
          this.diagnosticTestType = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllDiagnosticTestTypes()
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      }); */
  }

}
