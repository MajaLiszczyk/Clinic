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
import { LaboratoryTestType } from '../model/laboratory-test-type';
import { LaboratoryTest } from '../model/laboratory-test';
import { LaboratoryTestState } from '../model/laboratory-test';


@Component({
  selector: 'app-appointment-details',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink, CommonModule, FormsModule],
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css'
})

export class AppointmentDetailsComponent {
  //OTRZYMANE Z PARAMETRÓW
  laboratoryTestState = LaboratoryTestState;

  appointmentId: number = 0;
  isEditable: boolean = false; // wizyty zakończone/anulowane vs oczekujące 
  isCancelClicked: boolean = false;
  isAppointmentCancelled: boolean = false;

  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview: '', diagnosis: ''
                                            , isFinished: false, isCancelled: false, cancellingComment: '' };
  medicalAppointmentForm: FormGroup;
  cancelAppointmentForm: FormGroup;

  chooseDiagnosticTestTypeForm: FormGroup;
  chooseLaboratoryTestTypeForm: FormGroup;

  diagnosticTestForm: FormGroup;
  laboratoryTestForm: FormGroup;


  diagnosticTestTypes: DiagnosticTestType[] = [];
  laboratoryTestTypes: LaboratoryTestType[] = [];

  isDisabled: boolean = true; //ZMIENIĆ NAZWĘ przycisk select niedostępny póki nie zostanie wybrany typ badania.
  selectedDiagnosticTestType: DiagnosticTestType;
  selectedLaboratoryTestType: LaboratoryTestType;

  isDiagnosticTestVisible: boolean = false;
  isDiagnosticTestAddingMode: boolean = false;
  isDiagnosticTestEditableMode: boolean = false;
  diagnosticTestsTempList: DiagnosticTest[] = []; //chyba niepotrzebne
  pastDiagnosticTests: DiagnosticTest[] = [];
  editableDiagnosticTest: DiagnosticTest;
  listCounter: number = 0;

  isLaboratoryTestVisible: boolean = false;
  isLaboratoryTestAddingMode: boolean = false;
  isLaboratoryTestEditableMode: boolean = false;
  laboratoryTestsTempList: LaboratoryTest[] = []; //chyba niepotrzebne
  pastLaboratoryTests: LaboratoryTest[] = [];
  editableLaboratoryTest: LaboratoryTest;
  listLabotatoryTestCounter: number = 0;

  constructor(private http: HttpClient, private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router, private clinicService: ClinicService) {
    this.selectedDiagnosticTestType = { id: 0, name: '', isAvailable: true };
    this.selectedLaboratoryTestType = { id: 0, name: '', isAvailable: true };

    this.editableDiagnosticTest = { id: 0, medicalAppointmentId: this.appointmentId, diagnosticTestTypeId: 0
                                  , diagnosticTestTypeName: '', description: '' };
    this.editableLaboratoryTest = { id: 0, laboratoryTestsGroupId: 0, state: LaboratoryTestState.Comissioned, laboratoryTestTypeId: 0
                                  , laboratoryTestTypeName: '', result: '', doctorNote: '', rejectComment: '' };

    this.medicalAppointmentForm = this.fb.group({
      interviewText: new FormControl(null, { validators: [Validators.required] }), // Domyślna wartość
      diagnosisText: new FormControl(null, { validators: [Validators.required] })
    });
    this.cancelAppointmentForm = this.fb.group({
      cancelComment: new FormControl(null, { validators: [Validators.required] }),
    });
    this.chooseDiagnosticTestTypeForm = this.fb.group({});
    this.diagnosticTestForm = this.fb.group({});

    this.chooseLaboratoryTestTypeForm = this.fb.group({});
    this.laboratoryTestForm = this.fb.group({});
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.appointmentId = +params['id'];
      console.log('Received appointmentId:', this.appointmentId);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isEditable = queryParams['isEditable'] === 'true'; //czy wizyta z przeszlosci czy nie
      console.log('Is appointment editable:', this.isEditable);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isAppointmentCancelled = queryParams['isAppointmentCancelled'] === 'true';
      console.log('Is appointment cancelled:', this.isAppointmentCancelled);
    });
    this.getAllDiagnosticTestTypes()
    this.getAllLaboratoryTestTypes();
    //CHOOSE DIAGNOSTIC TEST TYPE FORM
    this.chooseDiagnosticTestTypeForm = this.fb.group({
      diagnosticTypeTestType: new FormControl(null, { validators: [Validators.required] })
    });
    this.chooseDiagnosticTestTypeForm.get('diagnosticTypeTestType')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Diagnostic test type selected:', value, 'isDisabled:', this.isDisabled);
    });
    //DIAGNOSTIC TEST FORM
    this.diagnosticTestForm = this.fb.group({
      diagnosticTestTypeName: new FormControl(null, { validators: [Validators.required] }), //ZMIENIC NA NULL? //chyba też required?
      description: new FormControl(null, { validators: [Validators.required] }),
    });

    //CHOOSE LABORATORY TEST TYPE FORM
    this.chooseLaboratoryTestTypeForm = this.fb.group({
      laboratoryTypeTestType: new FormControl(null, { validators: [Validators.required] })
    });
    this.chooseLaboratoryTestTypeForm.get('laboratoryTypeTestType')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Laboratory test type selected:', value, 'isDisabled:', this.isDisabled);
    });
    //LABORATORY TEST FORM
    this.laboratoryTestForm = this.fb.group({
      laboratoryTestTypeName: new FormControl(null, { validators: [Validators.required] }), //ZMIENIC NA NULL? //chyba też required?
      doctorNote: new FormControl(null, { validators: [Validators.required] }),
    });


    if (this.isEditable) {
      this.medicalAppointmentForm.get('interviewText')?.enable(); // Włączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.enable();
      this.cancelAppointmentForm.get('cancelComment')?.enable();
      this.diagnosticTestForm.get('diagnosticTestTypeName')?.disable();
      this.laboratoryTestForm.get('laboratoryTestTypeName')?.disable();
    }
    else {
      this.medicalAppointmentForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki
      this.cancelAppointmentForm.get('cancelComment')?.disable();
      this.diagnosticTestForm.get('diagnosticTestTypeName')?.disable();
      this.laboratoryTestForm.get('laboratoryTestTypeName')?.disable();

    }
    this.getMedicalAppointmentsDetails(this.appointmentId);
    console.log('Wywiad 2: ', this.medicalAppointment.interview);
    this.getDiagnosticTestsByAppointmentId();
    this.getLaboratoryTestsByAppointmentId(); //hehe po  grupie trzeba szukać?

  }

  get formInterview(): FormControl { return this.medicalAppointmentForm.get('interviewText') as FormControl };
  get formDiagnosis(): FormControl { return this.medicalAppointmentForm.get('diagnosisText') as FormControl; }

  get formTypeDiagnosticTestType(): FormControl { return this.chooseDiagnosticTestTypeForm?.get("diagnosticTypeTestType") as FormControl };
  get formDiagnosticTestType(): FormControl { return this.diagnosticTestForm.get('diagnosticTestTypeName') as FormControl; }
  get formDescription(): FormControl { return this.diagnosticTestForm.get('description') as FormControl; }

  get formTypeLaboratoryTestType(): FormControl { return this.chooseDiagnosticTestTypeForm?.get("laboratoryTestType") as FormControl };
  get formLaboratoryTestType(): FormControl { return this.diagnosticTestForm.get('laboratoryTestType') as FormControl; }
  get formDoctorNote(): FormControl { return this.diagnosticTestForm.get('doctorNote') as FormControl; }

  get formCancelComment(): FormControl { return this.cancelAppointmentForm.get('cancelComment') as FormControl; }


  selectDiagnosticTestType() {
    this.isDiagnosticTestAddingMode = true;
    this.isDiagnosticTestEditableMode = false;
    this.selectedDiagnosticTestType = this.chooseDiagnosticTestTypeForm.get('diagnosticTypeTestType')?.value;
    const testTypeName = this.selectedDiagnosticTestType.name;
    this.diagnosticTestForm.get('diagnosticTestTypeName')?.setValue(testTypeName);
  }

  selectLaboratoryTestType() {
    this.isLaboratoryTestAddingMode = true;
    this.isLaboratoryTestEditableMode = false;
    this.selectedLaboratoryTestType = this.chooseLaboratoryTestTypeForm.get('laboratoryTypeTestType')?.value;
    const testTypeName = this.selectedLaboratoryTestType.name;
    this.laboratoryTestForm.get('laboratoryTestTypeName')?.setValue(testTypeName);
  }

  getAllDiagnosticTestTypes() {
    this.clinicService.getAllDiagnosticTestTypes().subscribe(data => {
      this.diagnosticTestTypes = data;
    })
  }

  getAllLaboratoryTestTypes() {
    this.clinicService.getAllLaboratoryTestTypes().subscribe(data => {
      this.laboratoryTestTypes = data;
    })
  } 

  getDiagnosticTestsByAppointmentId() {
    this.clinicService.getDiagnosticTestsByAppointmentId(this.appointmentId).subscribe(data => {
      this.pastDiagnosticTests = data;
    })
  }

  getLaboratoryTestsByAppointmentId() {
    this.clinicService.getLaboratoryTestsByAppointmentId(this.appointmentId).subscribe(data => {
      this.pastLaboratoryTests = data;
    })
  }

  edit(diagnosticTest: DiagnosticTest) {
    this.editableDiagnosticTest = diagnosticTest;
    this.isDiagnosticTestEditableMode = true;
    this.isDiagnosticTestAddingMode = false; //niepotrzebne?
    this.formDescription.setValue(diagnosticTest.description);
    this.formDiagnosticTestType.setValue(diagnosticTest.diagnosticTestTypeName);
  }

  editLaboratoryTest(laboratoryTest: LaboratoryTest) {
    this.editableLaboratoryTest = laboratoryTest;
    this.isLaboratoryTestEditableMode = true;
    this.isLaboratoryTestAddingMode = false; //niepotrzebne?
    this.formDoctorNote.setValue(laboratoryTest.doctorNote);
    this.formLaboratoryTestType.setValue(laboratoryTest.laboratoryTestTypeName);
  }

  delete(diagnosticTestId: number) {
    var tempIndex = 0;
    for (let i = 0; i < this.pastDiagnosticTests.length; ++i) {
      if (this.pastDiagnosticTests[i].id == diagnosticTestId) {
        this.pastDiagnosticTests.splice(i, 1);
      }
    }
  }

  deleteLaboratoryTest(laboratoryTestId: number) {
    var tempIndex = 0;
    for (let i = 0; i < this.pastLaboratoryTests.length; ++i) {
      if (this.pastLaboratoryTests[i].id == laboratoryTestId) {
        this.pastLaboratoryTests.splice(i, 1);
      }
    }
  }

  cancelAddingDiagnosticTest() {
    this.isDiagnosticTestAddingMode = false;
    this.isDiagnosticTestEditableMode = false; //niepotrzebne?
    this.diagnosticTestForm.reset();
  }

  cancelAddingLaboratoryTest() {
    this.isLaboratoryTestAddingMode = false;
    this.isLaboratoryTestEditableMode = false; //niepotrzebne?
    this.laboratoryTestForm.reset();
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

  updateLaboratoryTest() {
    this.isLaboratoryTestAddingMode = false;
    this.isLaboratoryTestEditableMode = false; //niepotrzebne?
    const desc: string = this.laboratoryTestForm.get('description')?.value
    for (let i = 0; i < this.pastLaboratoryTests.length; ++i) {
      if (this.pastLaboratoryTests[i].id == this.editableLaboratoryTest.id) {
        this.pastLaboratoryTests[i].doctorNote = desc;
      }
    }
  }

  saveDiagnosticTest() {
    const desc: string = this.diagnosticTestForm.get('description')?.value
    const dTest: DiagnosticTest = { id: this.listCounter, medicalAppointmentId: this.appointmentId
                                  , diagnosticTestTypeId: this.selectedDiagnosticTestType.id
                                  , diagnosticTestTypeName: this.selectedDiagnosticTestType.name, description: desc };
    this.diagnosticTestsTempList.push(this.diagnosticTestForm.getRawValue());
    this.pastDiagnosticTests.push(dTest);
    this.diagnosticTestForm.reset();
    this.isDiagnosticTestAddingMode = false;
    this.listCounter++; //MYŚLĘ ŻE BARDZO ZŁA PRAKTYKA WYMYŚLANIA SZTUCZNYCH ID
  }

  saveLaboratoryTest() {
    const doctorNote: string = this.laboratoryTestForm.get('doctorNote')?.value
    const dTest: LaboratoryTest = { id: this.listCounter, laboratoryTestsGroupId: 0, state: LaboratoryTestState.Comissioned
                                  , result: '', rejectComment: '',  laboratoryTestTypeId: this.selectedLaboratoryTestType.id
                                  , laboratoryTestTypeName: this.selectedLaboratoryTestType.name, doctorNote: doctorNote };
    this.laboratoryTestsTempList.push(this.laboratoryTestForm.getRawValue());
    this.pastLaboratoryTests.push(dTest);
    this.laboratoryTestForm.reset();
    this.isLaboratoryTestAddingMode = false;
    this.listLabotatoryTestCounter++; //MYŚLĘ ŻE BARDZO ZŁA PRAKTYKA WYMYŚLANIA SZTUCZNYCH ID
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

  cancelAnAppointment() {
    this.isCancelClicked = true;
  }

  saveCancelComment() {
    if (this.cancelAppointmentForm.invalid) {
      this.cancelAppointmentForm.markAllAsTouched();
      return;
    }

    this.medicalAppointment.cancellingComment = this.cancelAppointmentForm.get('cancelComment')?.value || '';
    this.medicalAppointment.isCancelled = true;

    this.clinicService.editMedicalAppointmentCancel(this.medicalAppointment)
      .subscribe({
        next: (response) => {
          console.log("Operation completed successfully:", response);
          this.router.navigate(['/doctor-appointments/' + this.medicalAppointment.doctorId]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
    //DOROBIĆ WYSKAKUJĄCE OKIENKO
  }

  cancelCancelComment() {
    this.isCancelClicked = false
  }

  saveAnAppointment() {
    if (this.medicalAppointmentForm.invalid) {
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
        isFinished: true, //
        isCancelled: false,
        cancellingComment: this.medicalAppointment.cancellingComment
      },
      createDiagnosticTestDtos: this.pastDiagnosticTests.map(t => ({
        medicalAppointmentId: t.medicalAppointmentId,
        diagnosticTestTypeId: t.diagnosticTestTypeId,
        description: t.description,
      })),

      createLaboratoryTestDtos: this.pastLaboratoryTests.map(t => ({
        laboratoryTestTypeId: t.laboratoryTestTypeId,
        doctorNote: t.doctorNote,
      }))
    };

    //const headers = new HttpHeaders().set('Content-Type', 'application/json');
    this.clinicService.finishMedicalAppointment(finishAppointmentDto)
      .subscribe({
        next: (response) => {
          console.log("Operation completed successfully:", response);
          this.router.navigate(['/doctor-appointments/' + this.medicalAppointment.doctorId]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }
}