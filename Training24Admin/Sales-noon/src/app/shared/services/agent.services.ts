import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class AgentService {
    constructor(
        private commonApiService: CommonAPIService,
    ) { }

    getSalesAgentById(id?: string) {
        return this.commonApiService.get('v1/SalesAgent' + (id ? '/' + id : ''));
    }
    
    getSalesAgents(filterObj?: any) {
        return this.commonApiService.get('v1/SalesAgent', filterObj);
    }
    
    manageSalesAgent(data: any) {
        if(data.salesAgentId) {
            return this.commonApiService.put('v1/SalesAgent/'+data.salesAgentId, data)
        }
        else {
            return this.commonApiService.post('v1/SalesAgent', data);
        }
    }
    
    getAgentDeposit(filterObj?: any) {
        return this.commonApiService.get('v1/Deposit', filterObj);
    }
    
    getAgentDepositById(id?: string) {
        return this.commonApiService.get('v1/Deposit' + (id ? '/' + id : ''));
    }
    manageSalesAgentDeposit(data: any) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (key == 'documentid' && data[key]) {                
                for (let index = 0; index < data[key].length; index++) {
                    if (!data[key][index].documenturl)
                        formData.append('documentid', data[key][index]);
                }
            } else if (key == 'depositdate' && typeof data[key] == 'object') {
                data[key] = new Date(data[key]['year'], data[key]['month'] - 1, data[key]['day']).toISOString();
                formData.append(key, data[key]);
            } else
                formData.append(key, data[key]);
        });
        return this.commonApiService.post('v1/Deposit', formData);
    }

    getAccountSummary() {
        return this.commonApiService.get('v1/Deposit/GetAccountSummary');
    }

    deleteSalesAgent(id) {
        return this.commonApiService.delete('v1/SalesAgent/' + id);
    }

    getAgentCategoryById(id?: string) {
        return this.commonApiService.get('v1/AgentCategory' + (id ? '/' + id : ''));
    }

    getAgentCategory(filterObj?: any) {
        return this.commonApiService.get('v1/AgentCategory', filterObj);
    }

    manageAgentCategory(data: any) {
        return this.commonApiService.post('v1/AgentCategory', data);
    }

    deleteAgentCategory(id) {
        return this.commonApiService.delete('v1/AgentCategory/' + id);
    }
}