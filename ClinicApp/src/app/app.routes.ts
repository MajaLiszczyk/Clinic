import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { YourDataComponent } from './your-data/your-data.component';
import { AddPatientComponent } from './add-patient/add-patient.component';
import { AddDoctorComponent } from './add-doctor/add-doctor.component';
import { AddMedicalAppointmentComponent } from './add-medical-appointment/add-medical-appointment.component';
import { AddSpecialisationComponent } from './add-specialisation/add-specialisation.component';
import { MakeAnAppointmentComponent } from './make-an-appointment/make-an-appointment.component';

export const routes: Routes = [
    //{ path: '', component: AppComponent }, // Strona główna
    { path: 'your-data', component: YourDataComponent }, // Strona "your-data"
    { path: 'add-patient', component: AddPatientComponent }, // Strona "Add Patient"
    { path: 'add-doctor', component: AddDoctorComponent }, // Strona "Add Patient"
    { path: 'add-medical-appointment', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'add-specialisation', component: AddSpecialisationComponent},
    { path: 'your-data', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'make-an-appointment', component: MakeAnAppointmentComponent},
    { path: '**', redirectTo: '' }, // Przekierowanie na stronę główną dla nieznanych tras
    { path: '', redirectTo: '/your-data', pathMatch: 'full' }, // Przekierowanie do konkretnej trasy

];
