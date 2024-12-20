import { Component } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';



@Component({
  selector: 'app-appointment-details',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css'
})
export class AppointmentDetailsComponent {
  appointmentId: number = 0;
  medicalAppointment: MedicalAppointment = { id: 0, dateTime: new Date(), patientId: 0, doctorId: 0, interview:'', diagnosis: '', diseaseUnit: 0 };
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  isEditable: boolean = false;

  myForm: FormGroup;


  constructor(private http:HttpClient, private route: ActivatedRoute, private fb: FormBuilder){
    this.myForm = this.fb.group({
      interviewText: new FormControl('', {validators: [Validators.required]}), // Domyślna wartość
      diagnosisText: [''],
    });
  }


  /*ngOnInit(){
    this.route.params.subscribe(params => {
      this.medicalAppointment = params['medicalAppointment']; // Przypisanie id z URL
      console.log('Received Med Ap', this.medicalAppointment);
    });
    this.getMedicalAppointmentsForDoctor(this.medicalAppointment.id)
  } */

    ngOnInit() {
      this.route.params.subscribe(params => {
        this.appointmentId = +params['id'];
        console.log('papaap Received appointmentId:', this.appointmentId);

      });
    
      this.route.queryParams.subscribe(queryParams => {
        this.isEditable = queryParams['isEditable'] === 'true';
        
        console.log('Is appointment editable:', this.isEditable);
      });



      if (this.isEditable) {
        this.myForm.get('interviewText')?.enable(); // Włączanie kontrolki
        this.myForm.get('diagnosisText')?.enable();
        
      } else {
        this.myForm.get('interviewText')?.disable(); // Wyłączanie kontrolki
        this.myForm.get('diagnosisText')?.disable(); // Wyłączanie kontrolki
        

      }

      /*this.http.get<MedicalAppointment>(this.APIUrl+"/Get/" + this.appointmentId).subscribe(data =>{
        this.medicalAppointment=data;
        this.fillForm();
      }) */
    
      this.getMedicalAppointmentsDetails(this.appointmentId);
      console.log('Wywiad 2: ', this.medicalAppointment.interview);


      //this.formInterview.setValue(this.medicalAppointment.interview);

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
    return this.myForm.get('interviewText') as FormControl};

  //get formDiagnosis(): string {return this.myForm.get('diagnosisText')?.value;}
  get formDiagnosis(): FormControl {return this.myForm.get('diagnosisText') as FormControl;}



  getMedicalAppointmentsDetails(appointmentId: number){
    this.http.get<MedicalAppointment>(this.APIUrl+"/Get/" + appointmentId).subscribe(data =>{
      this.medicalAppointment=data;
      console.log('Wywiad: ', this.medicalAppointment.interview);
      //this.formInterview.get('interviewText')?.setValue(this.medicalAppointment.interview);
      this.fillForm();
    })
  }

  fillForm(){
    this.formInterview.setValue(this.medicalAppointment.interview);
    this.formDiagnosis.setValue(this.medicalAppointment.diagnosis);
  }

  saveAnAppointment(){
    this.medicalAppointment.diagnosis = this.formDiagnosis.value;
    this.medicalAppointment.interview = this.formInterview.value;
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    this.http.put<MedicalAppointment>(this.APIUrl+"/update", JSON.stringify(this.medicalAppointment), { headers })
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
