export interface ReturnMedicalAppointment{
    id: number;
    dateTime: Date;
    patientId: number;
    doctorId: number;
    interview: string;
    diagnosis: string;
    diseaseUnit: number;
}