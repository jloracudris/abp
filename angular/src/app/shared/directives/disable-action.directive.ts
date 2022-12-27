import { PermissionService } from '@abp/ng.core';
import { Directive, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appDisableAction]'
})
export class DisableActionDirective implements OnInit {

  @Input("appDisableAction")
  policy = '';

  constructor(private el: ElementRef, private renderer: Renderer2, private permissionService: PermissionService) {
    
  }

  ngOnInit(): void {    
    setTimeout(() => {
      const canCreate = this.permissionService.getGrantedPolicy(this.policy);
      this.renderer.setProperty(this.el.nativeElement, "disabled", !canCreate);
      this.el.nativeElement.disabled = !canCreate;  
    });    
  }

}
