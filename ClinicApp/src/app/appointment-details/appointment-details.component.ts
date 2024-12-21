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
  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview: '', diagnosis: '', diseaseUnit: 0 };
  readonly APIUrl = "https://localhost:5001/api/MedicalAppointment";
  isEditable: boolean = false;
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
  diagnosticTestsTemp: DiagnosticTest[] = [];
  diagnosticTests: DiagnosticTest[] = [];



  constructor(private http: HttpClient, private route: ActivatedRoute, private fb: FormBuilder) {
    this.selectedDiagnosticTestType = {id: 0, name: ''};
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
    this.chooseDiagnosticTestTypeForm = this.fb.group({
      diagnosticTestType: new FormControl(null, {validators: [Validators.required]})
    });
    this.diagnosticTestForm = this.fb.group({
      diagnosticTestType: new FormControl(null, {validators: [Validators.required]}),
      description: new FormControl(null, {validators: [Validators.required]})
    });
    if (this.isEditable) {
      this.medicalAppointmentForm.get('interviewText')?.enable(); // Włączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.enable();
    } else {
      this.medicalAppointmentForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki
    }
    this.chooseDiagnosticTestTypeForm.get('diagnosticTestType')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Diagnostic test type selected:', value, 'isDisabled:', this.isDisabled);
    });
    this.getMedicalAppointmentsDetails(this.appointmentId);
    console.log('Wywiad 2: ', this.medicalAppointment.interview);
  }


  selectDiagnosticTestType(){
    this.isDiagnosticTestAddingMode = true;
    this.isDiagnosticTestEditableMode = false;
    this.selectedDiagnosticTestType = this.formDiagnosticTestType.value;
    //this.isDiagnosticTestVisible = true; 
    this.formDiagnosticTestType.setValue(this.formTypeDiagnosticTestType);
  }

  getAllDiagnosticTestTypes(){
    this.http.get<DiagnosticTestType[]>("https://localhost:5001/api/DiagnosticTestType/Get").subscribe(data =>{
      this.diagnosticTestTypes=data;
    })
  }

  getDiagnosticTestsByAppointmentId(){
    this.http.get<DiagnosticTest[]>("https://localhost:5001/api/DiagnosticTest/Get/" + this.appointmentId).subscribe(data =>{
      this.diagnosticTests=data;
    })
  }

  edit(diagnosticTest: DiagnosticTest){

  }

  delete(diagnosticTestId: number){
    var tempIndex = 0;
    for (let i=0; i < this.diagnosticTestsTemp.length; ++i) {
      if(this.diagnosticTestsTemp[i].id == diagnosticTestId){
        this.diagnosticTestsTemp.splice(i, 1);
      }

    }
  }

  cancelAddingDiagnosticTest() {
    //this.isDiagnosticTestVisible = false;
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  updateDiagnosticTest() {
    //this.isFormVisible = false;
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  get formInterview(): FormControl {return this.medicalAppointmentForm.get('interviewText') as FormControl};
  get formDiagnosis(): FormControl { return this.medicalAppointmentForm.get('diagnosisText') as FormControl; }
  get formTypeDiagnosticTestType(): FormControl {return this.chooseDiagnosticTestTypeForm?.get("diagnosticTestType") as FormControl};
  get formDiagnosticTestType(): FormControl { return this.diagnosticTestForm.get('diagnosticTestType') as FormControl; }
  get formDescription(): FormControl { return this.diagnosticTestForm.get('description') as FormControl; }

  saveDiagnosticTest(){
    this.diagnosticTestsTemp.push(this.diagnosticTestForm.getRawValue());
    this.isDiagnosticTestAddingMode = false;
  }







  // Dodawanie nowego formularza - TWORZYMY NOWE BADANIE
  /*addForm(testType: DiagnosticTestType): void {
    const formGroup = this.fb.group({
      id: Number,
      medicalAppoitmentId: [this.appointmentId], 
      diagnosticTestTypeId: [testType.id, Validators.required], // Typ badania (readonly)
      diagnosticTestTypeName : [testType.name], // Czy mogę sobie dodać?
      description: [''], // Pole na opis 
    });
    console.log('Typ testu parametr: ' , testType); 
    console.log('Form group: ', formGroup);
    this.formsArray.push(formGroup);
  } */

  /*get formDoctorId(): FormControl {return this.medicalAppointmentForm?.get("doctorId") as FormControl};
  get formMedicalAppointmentDate(): FormControl {return this.medicalAppointmentForm.get("date") as FormControl};
  get timeControl(): FormControl {return this.medicalAppointmentForm.get("time") as FormControl};*/

  // Usuwanie formularza
  /*removeForm(index: number): void {
    this.formsArray.removeAt(index);
  }

  // Zapisanie formularzy
  submitForms(): void {
    for (let control of Object.values(this.mainForm.controls)) {
      console.log('kontrolki: ' , control);
    }

    console.log(this.mainForm.value); // Wyświetlenie w konsoli
  } */











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

  saveAnAppointment() {
    this.medicalAppointment.diagnosis = this.formDiagnosis.value;
    this.medicalAppointment.interview = this.formInterview.value;
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

  }

}
