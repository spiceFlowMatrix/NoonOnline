import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceAdminComponent } from './device-admin.component';

describe('DeviceAdminComponent', () => {
  let component: DeviceAdminComponent;
  let fixture: ComponentFixture<DeviceAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
