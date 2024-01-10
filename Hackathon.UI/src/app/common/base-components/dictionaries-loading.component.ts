import { Injectable } from '@angular/core';

@Injectable()
export abstract class DictionariesLoading {
  private dictionariesLoading: Map<string, boolean> = new Map<string, boolean>();

  public isDictLoading(name: string): boolean {
    return this.dictionariesLoading.has(name);
  }

  public changeLoadingStatus(name: string): void {
    if (!this.isDictLoading(name)) {
      this.dictionariesLoading.set(name, true);
    } else {
      this.dictionariesLoading.delete(name);
    }
  }
}
