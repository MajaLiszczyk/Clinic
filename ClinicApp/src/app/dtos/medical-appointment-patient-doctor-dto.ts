export interface MedicalAppointmentPatientDoctorDto{
    id: number;
    dateTime: string;
    patientId: number;
    patientName: string;
    patientSurname: string;
    patientPesel: string;
    doctorId: number;
    doctorName: string;
    doctorSurname: string;
    interview: string | undefined;
    diagnosis: string | undefined;
    isFinished: boolean | undefined;
    isCancelled: boolean | undefined; 
    cancelComment: string | null;

}