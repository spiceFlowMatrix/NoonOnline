import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceAdministartionComponent } from './device-administartion.component';

describe('DeviceAdministartionComponent', () => {
  let component: DeviceAdministartionComponent;
  let fixture: ComponentFixture<DeviceAdministartionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceAdministartionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceAdministartionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
