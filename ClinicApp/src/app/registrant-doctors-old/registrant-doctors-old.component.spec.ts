import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantDoctorsOldComponent } from './registrant-doctors-old.component';

describe('RegistrantDoctorsComponent', () => {
  let component: RegistrantDoctorsOldComponent;
  let fixture: ComponentFixture<RegistrantDoctorsOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantDoctorsOldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantDoctorsOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
