export interface MedicalAppointment{
    id: number;
    dateTime: Date;
    patientId: number;
    doctorId: number;
    interview: string;
    diagnosis: string;
    diseaseUnit: number;
    isFinished: boolean;
    isCancelled: boolean;
}