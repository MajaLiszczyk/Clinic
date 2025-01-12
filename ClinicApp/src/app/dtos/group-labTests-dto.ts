import { LaboratoryTest } from "../model/laboratory-test";

export interface GroupWithLabTests{
    groupId: number;
    laboratoryTests: LaboratoryTest[];
}
