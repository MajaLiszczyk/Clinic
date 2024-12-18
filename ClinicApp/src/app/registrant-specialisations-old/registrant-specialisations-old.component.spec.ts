import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantSpecialisationsComponent } from './registrant-specialisations-old.component';

describe('RegistrantSpecialisationsComponent', () => {
  let component: RegistrantSpecialisationsComponent;
  let fixture: ComponentFixture<RegistrantSpecialisationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantSpecialisationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantSpecialisationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
