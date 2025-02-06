import { MedicalAppointmentPatientDoctorDto } from "../dtos/medical-appointment-patient-doctor-dto";
import { MedicalAppointment } from "./medical-appointment";

export interface AllMedicalAppointments{
    pastMedicalAppointments: MedicalAppointmentPatientDoctorDto[];
    futureMedicalAppointments: MedicalAppointmentPatientDoctorDto[];
}