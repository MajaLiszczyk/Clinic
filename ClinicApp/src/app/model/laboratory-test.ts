export interface LaboratoryTest{
    id: number;
    laboratoryTestsGroupId: number;
    state: LaboratoryTestState;
    laboratoryTestTypeId: number;
    laboratoryTestTypeName: string;
    result?: string;
    doctorNote?: string;
    rejectComment: string;
}

export enum LaboratoryTestState {
    Comissioned = 0,
    ToBeCompleted = 1,
    Completed = 2,
    WaitingForSupervisor = 3,
    Accepted = 4,
    Rejected = 5
}