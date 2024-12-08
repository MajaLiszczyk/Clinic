import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRegistrantComponent } from './add-registrant.component';

describe('AddRegistrantComponent', () => {
  let component: AddRegistrantComponent;
  let fixture: ComponentFixture<AddRegistrantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddRegistrantComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddRegistrantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
