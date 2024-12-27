import { Component } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DiagnosticTestType } from '../model/diagnostic-test-type';
import { DiagnosticTest } from '../model/diagnostic-test';
import { ClinicService } from '../services/clinic.service';




@Component({
  selector: 'app-appointment-details',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink, CommonModule, FormsModule],
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css'
})
export class AppointmentDetailsComponent {
  //OTRZYMANE Z PARAMETRÓW
  appointmentId: number = 0;
  isEditable: boolean = false; // wizyty zakończone/anulowane vs oczekujące 
  isCancelClicked: boolean = false;
  isAppointmentCancelled: boolean = false;

  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview: '', diagnosis: '', isFinished: false, isCancelled: false, cancellingComment: '' };
  medicalAppointmentForm: FormGroup;
  cancelAppointmentForm: FormGroup;

  chooseDiagnosticTestTypeForm: FormGroup;
  diagnosticTestForm: FormGroup;

  diagnosticTestTypes: DiagnosticTestType[] = [];
  isDisabled: boolean = true; //ZMIENIĆ NAZWĘ przycisk select niedostępny póki nie zostanie wybrany typ badania.
  selectedDiagnosticTestType: DiagnosticTestType;
  isDiagnosticTestVisible: boolean = false;
  isDiagnosticTestAddingMode: boolean = false;
  isDiagnosticTestEditableMode: boolean = false;
  diagnosticTestsTempList: DiagnosticTest[] = []; //chyba niepotrzebne
  pastDiagnosticTests: DiagnosticTest[] = [];
  editableDiagnosticTest: DiagnosticTest;
  listCounter: number = 0;

  constructor(private http: HttpClient, private route: ActivatedRoute, private fb: FormBuilder,
              private router: Router, private clinicService: ClinicService) {
    this.selectedDiagnosticTestType = { id: 0, name: '' , isAvailable: true};
    this.editableDiagnosticTest = { id: 0, medicalAppointmentId: this.appointmentId, diagnosticTestTypeId: 0, diagnosticTestTypeName: '', description: '' };
    this.medicalAppointmentForm = this.fb.group({
      //interviewText: new FormControl('', { validators: [Validators.required] }), // Domyślna wartość
      interviewText: new FormControl('', { validators: [Validators.minLength(1), Validators.required] }), // Domyślna wartość
      //diagnosisText: [''],
      diagnosisText: new FormControl('', { validators: [Validators.minLength(1), Validators.required] })
    });
    this.cancelAppointmentForm = this.fb.group({
      cancelComment: new FormControl('', { validators: [Validators.minLength(1), Validators.required] }),
    });    
    this.chooseDiagnosticTestTypeForm = this.fb.group({});
    this.diagnosticTestForm = this.fb.group({});
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.appointmentId = +params['id'];
      console.log('Received appointmentId:', this.appointmentId);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isEditable = queryParams['isEditable'] === 'true';
      console.log('Is appointment editable:', this.isEditable);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isAppointmentCancelled = queryParams['isAppointmentCancelled'] === 'true';
      console.log('Is appointment cancelled:', this.isAppointmentCancelled);
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
      this.cancelAppointmentForm.get('cancelComment')?.enable();

    } else {
      this.medicalAppointmentForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki
      this.cancelAppointmentForm.get('cancelComment')?.disable();

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

  get formCancelComment(): FormControl { return this.cancelAppointmentForm.get('cancelComment') as FormControl; }


  selectDiagnosticTestType() {
    this.isDiagnosticTestAddingMode = true;
    this.isDiagnosticTestEditableMode = false;
    this.selectedDiagnosticTestType = this.chooseDiagnosticTestTypeForm.get('diagnosticTypeTestType')?.value;
    const testTypeName = this.selectedDiagnosticTestType.name;
    this.diagnosticTestForm.get('diagnosticTestTypeName')?.setValue(testTypeName);
  }

  getAllDiagnosticTestTypes() {
    //this.http.get<DiagnosticTestType[]>("https://localhost:5001/api/DiagnosticTestType/Get").subscribe(data => {
    this.clinicService.getAllDiagnosticTestTypes().subscribe(data => {
      this.diagnosticTestTypes = data;
    })
  }

  getDiagnosticTestsByAppointmentId() {
    //this.http.get<DiagnosticTest[]>("https://localhost:5001/api/DiagnosticTest/GetByMedicalAppointmentId/" + this.appointmentId).subscribe(data => {
    this.clinicService.getDiagnosticTestsByAppointmentId(this.appointmentId).subscribe(data => {
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
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    this.diagnosticTestForm.reset();
  }

  updateDiagnosticTest() {
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    const desc: string = this.diagnosticTestForm.get('description')?.value
    for (let i = 0; i < this.pastDiagnosticTests.length; ++i) {
      if (this.pastDiagnosticTests[i].id == this.editableDiagnosticTest.id) {
        this.pastDiagnosticTests[i].description = desc;
      }
    }
  }


  saveDiagnosticTest() {
    const desc: string = this.diagnosticTestForm.get('description')?.value
    const dTest: DiagnosticTest = { id: this.listCounter, medicalAppointmentId: this.appointmentId, diagnosticTestTypeId: this.selectedDiagnosticTestType.id, diagnosticTestTypeName: this.selectedDiagnosticTestType.name, description: desc };
    this.diagnosticTestsTempList.push(this.diagnosticTestForm.getRawValue());
    this.pastDiagnosticTests.push(dTest);
    this.diagnosticTestForm.reset();
    this.isDiagnosticTestAddingMode = false;
    this.listCounter++; //MYŚLĘ ŻE BARDZO ZŁA PRAKTYKA WYMYŚLANIA SZTUCZNYCH ID
  }



//------------------------------------------MEDICAL APPOINTMENT------------------------------------------------
  getMedicalAppointmentsDetails(appointmentId: number) {
    //this.http.get<MedicalAppointment>(this.APIUrl + "/Get/" + appointmentId).subscribe(data => {
    this.clinicService.getMedicalAppointmentById(appointmentId).subscribe(data => {
      this.medicalAppointment = data;
      console.log('Wywiad: ', this.medicalAppointment.interview);
      this.fillForm();
    })
  }

  fillForm() {
    this.formInterview.setValue(this.medicalAppointment.interview);
    this.formDiagnosis.setValue(this.medicalAppointment.diagnosis);
    this.formCancelComment.setValue(this.medicalAppointment.cancellingComment);

  }

  cancelAnAppointment(){
    this.isCancelClicked = true;
  }

  saveCancelComment(){
    if(this.cancelAppointmentForm.invalid){
      this.cancelAppointmentForm.markAllAsTouched(); 
      return;
    }
    
    this.medicalAppointment.cancellingComment = this.cancelAppointmentForm.get('cancelComment')?.value || '';
    this.medicalAppointment.isCancelled = true;

    //const headers = new HttpHeaders().set('Content-Type', 'application/json');
    //this.http.put<MedicalAppointment>(this.APIUrl + "/update", this.medicalAppointment, { headers })
    this.clinicService.editMedicalAppointmentCancel(this.medicalAppointment)
      .subscribe({
        next: (response) => {
          console.log("Operation completed successfully:", response);
          this.router.navigate(['/doctor-appointments/'+ this.medicalAppointment.doctorId]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });

    //DOROBIĆ WYSKAKUJĄCE OKIENKO

    /*this.http.put<MedicalAppointment>(this.APIUrl + "/update", JSON.stringify(this.medicalAppointment), { headers })
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      }); */
  }

  cancelCancelComment(){
    this.isCancelClicked = false
  }

  saveAnAppointment() {
    if(this.medicalAppointmentForm.invalid){
      this.medicalAppointmentForm.markAllAsTouched(); 
      return;
    }
    const finishAppointmentDto = {
      medicalAppointmentDto: {
        id: this.medicalAppointment.id,
        dateTime: this.medicalAppointment.dateTime,
        patientId: this.medicalAppointment.patientId,
        doctorId: this.medicalAppointment.doctorId,
        interview: this.formInterview.value, //
        diagnosis: this.formDiagnosis.value, //
        //diseaseUnit: this.medicalAppointment.diseaseUnit,
        isFinished: true, //
        isCancelled: false,
        cancellingComment: this.medicalAppointment.cancellingComment
      },
      createDiagnosticTestDtos: this.pastDiagnosticTests.map(t => ({
        medicalAppointmentId: t.medicalAppointmentId,
        diagnosticTestTypeId: t.diagnosticTestTypeId,
        description: t.description,
      }))
    };
  
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    //this.http.post(this.APIUrl + "/FinishMedicalAppointment", finishAppointmentDto, { headers })
    //this.clinicService.finishMedicalAppointment(finishAppointmentDto, { headers })
    this.clinicService.finishMedicalAppointment(finishAppointmentDto)
      .subscribe({
        next: (response) => {
          console.log("Operation completed successfully:", response);
          this.router.navigate(['/doctor-appointments/'+ this.medicalAppointment.doctorId]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }



  /*saveAnAppointmentOld() {
    this.medicalAppointment.diagnosis = this.formDiagnosis.value;
    this.medicalAppointment.interview = this.formInterview.value;
    this.medicalAppointment.isFinished = true;
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
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
        medicalAppointmentId: t.medicalAppointmentId,
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
  } */
}
