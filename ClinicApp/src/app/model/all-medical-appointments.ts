import { MedicalAppointment } from "./medical-appointment";

export interface AllMedicalAppointments{
    pastMedicalAppointments: MedicalAppointment[];
    futureMedicalAppointments: MedicalAppointment[];
}