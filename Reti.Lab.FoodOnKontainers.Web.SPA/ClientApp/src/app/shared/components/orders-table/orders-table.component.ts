import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-orders-table',
    templateUrl: './orders-table.component.html',
})
export class OrdersTableComponent {
    @Input() items: any = [];
    @Input() tableClass: string;

    get total(): number {
        if (this.items) {
            return this.items.reduce((prev, next) => {
                return prev + (next.quantity * next.price);
            }, 0);
        }
        return 0;        
    }
}
