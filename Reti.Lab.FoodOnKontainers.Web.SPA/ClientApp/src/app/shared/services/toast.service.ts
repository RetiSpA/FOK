import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })

export class ToastService {
    toasts: any[] = [];

    show(body: string, header?: string, type?: ToastType, delay?: number) {
        const h = header || '';
        const b = body || '';
        let c = '';
        switch (type) {
            case ToastType.success:
                c = "bg-success text-light";
                break;
            case ToastType.warning:
                c = "bg-warning";
                break;
            case ToastType.danger:
                c = "bg-danger text-light";
                break;
            case ToastType.info:
                c = "bg-info text-light";
            default:
                break;
        }
        const d = delay || 4000;
        this.toasts.push({ b, h, c, d });
    }

    remove(toast) {
        this.toasts = this.toasts.filter(t => t != toast);
    }
}

export enum ToastType {
    info,
    success,
    warning,
    danger
}