export interface MedicalAppointment{
    id: number;
    dateTime: string;
    patientId: number;
    doctorId: number;
    interview: string;
    diagnosis: string;
    diseaseUnit: number;
}