import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaboratorySupervisorComponent } from './laboratory-supervisor.component';

describe('LaboratorySupervisorComponent', () => {
  let component: LaboratorySupervisorComponent;
  let fixture: ComponentFixture<LaboratorySupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaboratorySupervisorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LaboratorySupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
