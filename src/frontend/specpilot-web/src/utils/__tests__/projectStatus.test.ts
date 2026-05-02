import { describe, expect, it } from 'vitest';
import { getProjectStatusMeta } from '../projectStatus';

describe('getProjectStatusMeta', () => {
  it('retorna rotulo, dica e proximo passo para Draft', () => {
    expect(getProjectStatusMeta('Draft')).toEqual({
      label: 'Rascunho',
      hint: 'Projeto em rascunho. Gere perguntas de refinamento para continuar o fluxo principal.',
      nextStep: 'Gerar perguntas de refinamento'
    });
  });

  it('retorna rotulo, dica e proximo passo para DocumentGenerated', () => {
    expect(getProjectStatusMeta('DocumentGenerated')).toEqual({
      label: 'Documento gerado',
      hint: 'Documento tecnico inicial gerado. Revise o resultado final do fluxo do MVP.',
      nextStep: 'Visualizar documento tecnico'
    });
  });
});
