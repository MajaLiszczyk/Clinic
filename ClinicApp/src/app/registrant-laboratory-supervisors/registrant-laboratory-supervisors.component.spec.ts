import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantLaboratorySupervisorsComponent } from './registrant-laboratory-supervisors.component';

describe('RegistrantLaboratorySupervisorsComponent', () => {
  let component: RegistrantLaboratorySupervisorsComponent;
  let fixture: ComponentFixture<RegistrantLaboratorySupervisorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantLaboratorySupervisorsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantLaboratorySupervisorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
