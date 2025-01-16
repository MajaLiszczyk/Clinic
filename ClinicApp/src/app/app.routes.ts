
//import { RegistrantDoctorsComponent } from './registrant-doctors-old/registrant-doctors-old.component';
//import { RegistrantSpecialisationsOldComponent } from './registrant-specialisations-old/registrant-specialisations-old.component';
//import { PatientParentComponent } from './patient-parent/patient-parent.component';
//import { RegistrantPatientsOldComponent } from './registrant-patients-old/registrant-patients-old.component';
//import { YourDataComponent } from './your-data/your-data.component';
//import { AddPatientComponent } from './add-patient/add-patient.component';
//import { AddDoctorComponent } from './add-doctor/add-doctor.component';
//import { AddMedicalAppointmentComponent } from './add-medical-appointment/add-medical-appointment.component';
//import { AddSpecialisationComponent } from './add-specialisation/add-specialisation.component';
//import { GetPatientsComponent } from './get-patients/get-patients.component';
//import { GetDoctorsComponent } from './get-doctors/get-doctors.component';
//import { GetSpecialisationsComponent } from './get-specialisations/get-specialisations.component';
//import { GetMedicalAppointmentsComponent } from './get-medical-appointments/get-medical-appointments.component';
//import { GetRegistrantsComponent } from './get-registrants/get-registrants.component';
//import { AddRegistrantComponent } from './add-registrant/add-registrant.component';
//import { GetMedicalAppointmentsForPatientComponent } from './get-medical-appointments-for-patient/get-medical-appointments-for-patient.component';
//import { MakeAnAppointmentComponent } from './make-an-appointment/make-an-appointment.component'; //???????
//import { DoctorComponent } from './doctor/doctor.component';//?????

import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PatientComponent } from './patient/patient.component';
import { RegistrantPatientsComponent } from './registrant-patients/registrant-patients.component';
import { RegistrantComponent } from './registrant/registrant.component';
import { RegistrantSpecialisationsComponent } from './registrant-specialisations/registrant-specialisations.component';
import { RegistrantDiagnosticTestTypesComponent } from './registrant-diagnostic-test-types/registrant-diagnostic-test-types.component';
import { RegistrantDoctorsComponent } from './registrant-doctors/registrant-doctors.component';
import { RegistrantAppointmentsComponent } from './registrant-appointments/registrant-appointments.component';
import { DoctorAppointmentsComponent } from './doctor-appointments/doctor-appointments.component';
import { AppointmentDetailsComponent } from './appointment-details/appointment-details.component';
import { RegistrantLaboratoryWorkersComponent } from './registrant-laboratory-workers/registrant-laboratory-workers.component';
import { RegistrantLaboratorySupervisorsComponent } from './registrant-laboratory-supervisors/registrant-laboratory-supervisors.component';
import { RegistrantLaboratoryTestTypesComponent } from './registrant-laboratory-test-types/registrant-laboratory-test-types.component';
import { RegistrantLaboratoryAppointmentsComponent } from './registrant-laboratory-appointments/registrant-laboratory-appointments.component';
import { PatientMenuComponent } from './patient-menu/patient-menu.component';

import { LoginComponent } from './log-in/log-in.component';
import { AuthorizationGuard } from './authorization/authorization-guard';
import { PatientLaboratoryAppointmentsComponent } from './patient-laboratory-appointments/patient-laboratory-appointments.component';
import { LaboratoryWorkerComponent } from './laboratory-worker/laboratory-worker.component';


export const routes: Routes = [
    /*{ path: 'your-data', component: YourDataComponent }, // Strona "your-data"
    { path: 'add-patient', component: AddPatientComponent }, // Strona "Add Patient"
    { path: 'add-doctor', component: AddDoctorComponent }, // Strona "Add Patient"
    { path: 'add-medical-appointment', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'add-specialisation', component: AddSpecialisationComponent},
    { path: 'your-data', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'get-patients', component: GetPatientsComponent},
    { path: 'get-doctors', component: GetDoctorsComponent},
    { path: 'get-specialisations', component: GetSpecialisationsComponent},
    { path: 'get-medical-appointments', component: GetMedicalAppointmentsComponent},
    { path: 'get-registrants', component: GetRegistrantsComponent},
    { path: 'add-registrant', component: AddRegistrantComponent},
    { path: 'get-medical-appointments-for-patient', component: GetMedicalAppointmentsForPatientComponent},
    { path: 'patient-parent', component: PatientParentComponent},*/

    { path: '', component: LoginComponent  }, // Strona główna
    { path: 'log-in', component: LoginComponent },  
    //{ path: 'make-an-appointment', component: MakeAnAppointmentComponent, canActivate: [AuthorizationGuard] },
    //{ path: 'patient/:patientId', component: PatientComponent},
    { path: 'patient/:patientId', component: PatientComponent, canActivate: [AuthorizationGuard] },
    { path: 'patient-menu/:patientId', component: PatientMenuComponent, canActivate: [AuthorizationGuard] },
    { path: 'patient-laboratory-appointments/:patientId', component: PatientLaboratoryAppointmentsComponent, canActivate: [AuthorizationGuard] },


    { path: 'registrant', component: RegistrantComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-patients', component: RegistrantPatientsComponent, canActivate: [AuthorizationGuard] },
    //{ path: 'registrant-specialisations-old', component: RegistrantSpecialisationsOldComponent},
    { path: 'registrant-specialisations', component: RegistrantSpecialisationsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-diagnostic-test-types', component: RegistrantDiagnosticTestTypesComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-doctors', component: RegistrantDoctorsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-workers', component: RegistrantLaboratoryWorkersComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-supervisors', component: RegistrantLaboratorySupervisorsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-appointments', component: RegistrantAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-test-types', component: RegistrantLaboratoryTestTypesComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-appointments', component: RegistrantLaboratoryAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'laboratory-worker/:laboratoryWorkerId', component: LaboratoryWorkerComponent, canActivate: [AuthorizationGuard] },

    
    //{ path: 'doctor', component: DoctorComponent},
    { path: 'doctor-appointments/:doctorId', component: DoctorAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'appointment-details/:id', component: AppointmentDetailsComponent, },

    { path: '**', redirectTo: '' }, // Przekierowanie na stronę główną dla nieznanych tras
    { path: '', redirectTo: '/your-data', pathMatch: 'full' }, // Przekierowanie do konkretnej trasy

];
