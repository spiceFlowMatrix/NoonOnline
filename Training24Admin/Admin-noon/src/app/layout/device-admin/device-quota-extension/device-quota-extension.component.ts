import { Component, OnInit } from '@angular/core';
import { DeviceService } from '../../../shared/services/device.service';

const ELEMENT_DATA: any[] = [
  { Date: 1, Username: 'Hydrogen', 'Device_Id': 1.0079, 'Email_Address': 'H', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 2, Username: 'Helium', 'Device_Id': 4.0026, 'Email_Address': 'He', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 3, Username: 'Lithium', 'Device_Id': 6.941, 'Email_Address': 'Li', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 4, Username: 'Beryllium', 'Device_Id': 9.0122, 'Email_Address': 'Be', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 5, Username: 'Boron', 'Device_Id': 10.811, 'Email_Address': 'B', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 6, Username: 'Carbon', 'Device_Id': 12.0107, 'Email_Address': 'C', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 7, Username: 'Nitrogen', 'Device_Id': 14.0067, 'Email_Address': 'N', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 8, Username: 'Oxygen', 'Device_Id': 15.9994, 'Email_Address': 'O', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 9, Username: 'Fluorine', 'Device_Id': 18.9984, 'Email_Address': 'F', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
  { Date: 10, Username: 'Neon', 'Device_Id': 20.1797, 'Email_Address': 'Ne', 'Current_Quota_Limitation': 2, 'Requested_Quota_Limistation': 5, Status: 'Pending' },
];
@Component({
  selector: 'app-device-quota-extension',
  templateUrl: './device-quota-extension.component.html',
  styleUrls: ['./device-quota-extension.component.scss']
})
export class DeviceQuotaExtensionComponent implements OnInit {
  displayedColumns: string[] = ['Date', 'Username', 'Email Address', 'Device Id', 'Current Quota Limitation', 'Requested Quota Limistation', 'Status', 'reject', 'approve'];
  dataSource = ELEMENT_DATA;
  constructor(public deviceService: DeviceService) { }

  ngOnInit(): void {
    this.getRequest()
  }

  getRequest() {
    this.deviceService.extensionRequest().subscribe((res)=> {
      console.log(res);
      this.dataSource = res.data.deviceQuotaExtensionList;
    })
  }
  acceptReject(id,i,status) {
    this.deviceService.AcceptRejectRequest(id,status).subscribe((res)=> {
      console.log(res);
      if(status){
        this.dataSource[i].status = "Accepted"
      } else {
        this.dataSource[i].status = "Rejected"
      }
    })
  }
}
