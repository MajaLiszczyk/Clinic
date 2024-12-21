import { Component } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DiagnosticTestType } from '../model/diagnostic-test-type';



@Component({
  selector: 'app-appointment-details-old',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink, CommonModule, FormsModule],
  templateUrl: './appointment-details-old.component.html',
  styleUrl: './appointment-details-old.component.css'
})
export class AppointmentDetailsOldComponent {
  appointmentId: number = 0;
  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview: '', diagnosis: '', diseaseUnit: 0 };
  readonly APIUrl = "https://localhost:5001/api/MedicalAppointment";
  isEditable: boolean = false;
  medicalAppointmentForm: FormGroup;
  diagnosticTestTypes: DiagnosticTestType[] = [];
  //diagnosticTestTypes = ['Blood Test', 'X-Ray', 'MRI', 'CT Scan']; // Przykładowe typy badań
  formsArray: FormArray; // Tablica formularzy
  mainForm: FormGroup; // Główny formularz zawierający tablicę
  diagnosticTestType: DiagnosticTestType;



  constructor(private http: HttpClient, private route: ActivatedRoute, private fb: FormBuilder) {
    this.diagnosticTestType = {id: 0, name: ''};
    this.medicalAppointmentForm = this.fb.group({
      interviewText: new FormControl('', { validators: [Validators.required] }), // Domyślna wartość
      diagnosisText: [''],
    });

    this.mainForm = this.fb.group({
      formsArray: this.fb.array([]) // Tablica dynamicznych formularzy
    });
    this.formsArray = this.mainForm.get('formsArray') as FormArray;
  }

  // Dodawanie nowego formularza - TWORZYMY NOWE BADANIE
  addForm(testType: DiagnosticTestType): void {
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
    
  }

  /*get formDoctorId(): FormControl {return this.medicalAppointmentForm?.get("doctorId") as FormControl};
  get formMedicalAppointmentDate(): FormControl {return this.medicalAppointmentForm.get("date") as FormControl};
  get timeControl(): FormControl {return this.medicalAppointmentForm.get("time") as FormControl};*/

  // Usuwanie formularza
  removeForm(index: number): void {
    this.formsArray.removeAt(index);
  }

  // Zapisanie formularzy
  submitForms(): void {
    for (let control of Object.values(this.mainForm.controls)) {
      console.log('kontrolki: ' , control);
    }

    console.log(this.mainForm.value); // Wyświetlenie w konsoli
    console.log(this.mainForm.value); // Wyświetlenie w konsoli
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



    if (this.isEditable) {
      this.medicalAppointmentForm.get('interviewText')?.enable(); // Włączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.enable();

    } else {
      this.medicalAppointmentForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
      this.medicalAppointmentForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki


    }

    /*this.http.get<MedicalAppointment>(this.APIUrl+"/Get/" + this.appointmentId).subscribe(data =>{
      this.medicalAppointment=data;
      this.fillForm();
    }) */

    this.getMedicalAppointmentsDetails(this.appointmentId);
    console.log('Wywiad 2: ', this.medicalAppointment.interview);


    //this.formInterview.setValue(this.medicalAppointment.interview);

  }

  getAllDiagnosticTestTypes(){
    this.http.get<DiagnosticTestType[]>("https://localhost:5001/api/DiagnosticTestType/Get").subscribe(data =>{
      this.diagnosticTestTypes=data;
    })
  }


  /*ngOnInit(){
    console.log('Received med ap Id :', this.appointmentId);

    this.route.params.subscribe(params => {
      this.appointmentId = +params['appointmentId']; // Przypisanie id z URL
      console.log('papaap Received appointmentId:', this.appointmentId);
    });
    this.route.params.subscribe(params => {
      this.isEditable = params['isEditable']; // Przypisanie id z URL
      console.log('lalala Is appointment editable:', this.isEditable);
    });
    this.getMedicalAppointmentsDetails(this.appointmentId)

  } */


  get formInterview(): FormControl {
    //return this.myForm.get('interviewText')?.value;
    return this.medicalAppointmentForm.get('interviewText') as FormControl
  };

  //get formDiagnosis(): string {return this.myForm.get('diagnosisText')?.value;}
  get formDiagnosis(): FormControl { return this.medicalAppointmentForm.get('diagnosisText') as FormControl; }



  getMedicalAppointmentsDetails(appointmentId: number) {
    this.http.get<MedicalAppointment>(this.APIUrl + "/Get/" + appointmentId).subscribe(data => {
      this.medicalAppointment = data;
      console.log('Wywiad: ', this.medicalAppointment.interview);
      //this.formInterview.get('interviewText')?.setValue(this.medicalAppointment.interview);
      this.fillForm();
    })
  }

  /*getDiagnostiTestTypes() {
    this.http.get<DiagnosticTestType[]>("https://localhost:5001/api/DiagnosticTestType").subscribe(data => {
      this.diagnosticTestTypes = data;
    })
  } */


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



  /*cancelAnAppointment(){
    this.myForm.reset();

  } */


}
