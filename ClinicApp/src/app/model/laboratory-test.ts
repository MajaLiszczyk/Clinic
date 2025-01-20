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
    WaitingForSupervisor = 2,
    Accepted = 3,
    Rejected = 4
}