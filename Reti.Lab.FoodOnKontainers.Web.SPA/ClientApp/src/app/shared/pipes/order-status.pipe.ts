import { Pipe, PipeTransform } from '@angular/core';
import { OrderStatus } from '../models/order';

@Pipe({
  name: 'orderStatus'
})
export class OrderStatusPipe implements PipeTransform {
  transform(value: OrderStatus): string {
    return OrderStatus[value];
  }
}
