import { Component } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DiagnosticTestType } from '../model/diagnostic-test-type';
import { DiagnosticTest } from '../model/diagnostic-test';




@Component({
  selector: 'app-appointment-details',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink, CommonModule, FormsModule],
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css'
})
export class AppointmentDetailsComponent {
  appointmentId: number = 0;
  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview: '', diagnosis: '', diseaseUnit: 0, isFinished: false, isCancelled: false };
  readonly APIUrl = "https://localhost:5001/api/MedicalAppointment";
  isEditable: boolean = false; // wizyty z przeszłości vs z przyszłości 
  medicalAppointmentForm: FormGroup;
  diagnosticTestTypes: DiagnosticTestType[] = [];
  //formsArray: FormArray; // Tablica formularzy
  //mainForm: FormGroup; // Główny formularz zawierający tablicę
  //diagnosticTestType: DiagnosticTestType;
  chooseDiagnosticTestTypeForm: FormGroup;
  isDisabled: boolean = true;
  selectedDiagnosticTestType: DiagnosticTestType;
  isDiagnosticTestVisible: boolean = false;
  diagnosticTestForm: FormGroup;
  isDiagnosticTestAddingMode: boolean = false;
  isDiagnosticTestEditableMode: boolean = false;
  diagnosticTestsTempList: DiagnosticTest[] = [];
  pastDiagnosticTests: DiagnosticTest[] = [];
  editableDiagnosticTest: DiagnosticTest;
  listCounter: number = 0;



  constructor(private http: HttpClient, private route: ActivatedRoute, private fb: FormBuilder) {
    this.selectedDiagnosticTestType = { id: 0, name: '' };
    this.editableDiagnosticTest = { id: 0, medicalAppointmentId: this.appointmentId, diagnosticTestTypeId: 0, diagnosticTestTypeName: '', description: '' };
    //this.diagnosticTestType = {id: 0, name: ''};
    this.medicalAppointmentForm = this.fb.group({
      interviewText: new FormControl('', { validators: [Validators.required] }), // Domyślna wartość
      diagnosisText: [''],
    });
    this.chooseDiagnosticTestTypeForm = this.fb.group({});
    this.diagnosticTestForm = this.fb.group({});
    /*this.mainForm = this.fb.group({
      formsArray: this.fb.array([]) // Tablica dynamicznych formularzy
    });
    this.formsArray = this.mainForm.get('formsArray') as FormArray; */
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.appointmentId = +params['id'];
      console.log('papaap Received appointmentId:', this.appointmentId);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isEditable = queryParams['isEditable'] === 'true';
      console.log('Is appointment editable:', this.isEditable);
    });
    this.getAllDiagnosticTestTypes()
    //CHOOSE TYPE FORM
    this.chooseDiagnosticTestTypeForm = this.fb.group({
      diagnosticTypeTestType: new FormControl(null, { validators: [Validators.required] })
    });
    this.chooseDiagnosticTestTypeForm.get('diagnosticTypeTestType')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Diagnostic test type selected:', value, 'isDisabled:', this.isDisabled);
    });
    //DIAGNOSTIC TEST FORM
    this.diagnosticTestForm = this.fb.group({
      diagnosticTestTypeName: new FormControl(''),
      description: new FormControl('', { validators: [Validators.required] }),
    });

    if (this.isEditable) {
      this.medicalAppointmentForm.get('interviewText')?.enable(); // Włączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.enable();
    } else {
      this.medicalAppointmentForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki
    }
    this.getMedicalAppointmentsDetails(this.appointmentId);
    console.log('Wywiad 2: ', this.medicalAppointment.interview);
    this.getDiagnosticTestsByAppointmentId();
  }

  get formInterview(): FormControl { return this.medicalAppointmentForm.get('interviewText') as FormControl };
  get formDiagnosis(): FormControl { return this.medicalAppointmentForm.get('diagnosisText') as FormControl; }
  get formTypeDiagnosticTestType(): FormControl { return this.chooseDiagnosticTestTypeForm?.get("diagnosticTypeTestType") as FormControl };
  get formDiagnosticTestType(): FormControl { return this.diagnosticTestForm.get('diagnosticTestTypeName') as FormControl; }
  get formDescription(): FormControl { return this.diagnosticTestForm.get('description') as FormControl; }

  selectDiagnosticTestType() {
    this.isDiagnosticTestAddingMode = true;
    this.isDiagnosticTestEditableMode = false;
    this.selectedDiagnosticTestType = this.chooseDiagnosticTestTypeForm.get('diagnosticTypeTestType')?.value;
    const testTypeName = this.selectedDiagnosticTestType.name;
    this.diagnosticTestForm.get('diagnosticTestTypeName')?.setValue(testTypeName);
  }

  getAllDiagnosticTestTypes() {
    this.http.get<DiagnosticTestType[]>("https://localhost:5001/api/DiagnosticTestType/Get").subscribe(data => {
      this.diagnosticTestTypes = data;
    })
  }

  getDiagnosticTestsByAppointmentId() {
    this.http.get<DiagnosticTest[]>("https://localhost:5001/api/DiagnosticTest/GetByMedicalAppointmentId/" + this.appointmentId).subscribe(data => {
      this.pastDiagnosticTests = data;
    })
  }

  edit(diagnosticTest: DiagnosticTest) {
    this.editableDiagnosticTest = diagnosticTest;
    this.isDiagnosticTestEditableMode = true;
    this.isDiagnosticTestAddingMode = false; //niepotrzebne?
    this.formDescription.setValue(diagnosticTest.description);
    this.formDiagnosticTestType.setValue(diagnosticTest.diagnosticTestTypeName);

  }

  delete(diagnosticTestId: number) {
    var tempIndex = 0;
    for (let i = 0; i < this.pastDiagnosticTests.length; ++i) {
      if (this.pastDiagnosticTests[i].id == diagnosticTestId) {
        this.pastDiagnosticTests.splice(i, 1);
      }
    }
  }

  cancelAddingDiagnosticTest() {
    //this.isDiagnosticTestVisible = false;
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    this.diagnosticTestForm.reset();

    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  updateDiagnosticTest() {
    //this.isFormVisible = false;
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
    const desc: string = this.diagnosticTestForm.get('description')?.value
    for (let i = 0; i < this.pastDiagnosticTests.length; ++i) {
      if (this.pastDiagnosticTests[i].id == this.editableDiagnosticTest.id) {
        this.pastDiagnosticTests[i].description = desc;
      }
    }
    /*const requestBody = {
      ...this.doctorForm.getRawValue(),
      specialisationsList: this.doctorSpecialisationsList
    }; */
  }


  saveDiagnosticTest() {
    const desc: string = this.diagnosticTestForm.get('description')?.value
    const dTest: DiagnosticTest = { id: this.listCounter, medicalAppointmentId: this.appointmentId, diagnosticTestTypeId: this.selectedDiagnosticTestType.id, diagnosticTestTypeName: this.selectedDiagnosticTestType.name, description: desc };
    this.diagnosticTestsTempList.push(this.diagnosticTestForm.getRawValue());
    this.pastDiagnosticTests.push(dTest);
    this.diagnosticTestForm.reset();
    this.isDiagnosticTestAddingMode = false;
    this.listCounter++; //MYŚLĘ ŻE BARDZO ZŁA PRAKTYKA WYMYŚLANIA SZTUCZNYCH ID
    //this.isDisabled = true; //niemozliwosc Select przy wybraniu typu badania

  }









  getMedicalAppointmentsDetails(appointmentId: number) {
    this.http.get<MedicalAppointment>(this.APIUrl + "/Get/" + appointmentId).subscribe(data => {
      this.medicalAppointment = data;
      console.log('Wywiad: ', this.medicalAppointment.interview);
      //this.formInterview.get('interviewText')?.setValue(this.medicalAppointment.interview);
      this.fillForm();
    })
  }

  fillForm() {
    this.formInterview.setValue(this.medicalAppointment.interview);
    this.formDiagnosis.setValue(this.medicalAppointment.diagnosis);
  }

  cancelAnAppointment(){
    this.medicalAppointment.isCancelled = true;

  }

  saveAnAppointment() {
    this.medicalAppointment.diagnosis = this.formDiagnosis.value;
    this.medicalAppointment.interview = this.formInterview.value;
    this.medicalAppointment.isFinished = true;

    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    //TRZEBA WYKONAC TEŻ DRUGIE ZAPYTANIE Z BADANIAMI

    this.http.put<MedicalAppointment>(this.APIUrl + "/update", JSON.stringify(this.medicalAppointment), { headers })
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });

    for (let t of this.pastDiagnosticTests){

      const requestBody = {
        //id: 0,
        medicalAppoitmentId: t.medicalAppointmentId,
        diagnosticTestTypeId: t.diagnosticTestTypeId,
        description: t.description,
      };
      this.http.post<DiagnosticTest>("https://localhost:5001/api/DiagnosticTest/create", requestBody)
        .subscribe({
          next: (response) => {
            console.log("Action performed successfully:", response);
          },
          error: (error) => {
            console.error("Error performing action:", error);
          }
        })
    }
    

  }

}
