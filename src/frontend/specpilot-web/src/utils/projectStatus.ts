import type { ProjectStatus } from '../types/api';

interface ProjectStatusMeta {
  label: string;
  hint: string;
  nextStep: string;
}

const projectStatusMeta: Record<ProjectStatus, ProjectStatusMeta> = {
  Draft: {
    label: 'Rascunho',
    hint: 'Projeto em rascunho. Gere perguntas de refinamento para continuar o fluxo principal.',
    nextStep: 'Gerar perguntas de refinamento'
  },
  QuestionsGenerated: {
    label: 'Perguntas geradas',
    hint: 'Perguntas de refinamento prontas. Responda todas para liberar a geracao do documento.',
    nextStep: 'Responder perguntas de refinamento'
  },
  QuestionsAnswered: {
    label: 'Perguntas respondidas',
    hint: 'Refinamento concluido. Gere o documento tecnico inicial para finalizar a jornada principal.',
    nextStep: 'Gerar documento tecnico'
  },
  DocumentGenerated: {
    label: 'Documento gerado',
    hint: 'Documento tecnico inicial gerado. Revise o resultado final do fluxo do MVP.',
    nextStep: 'Visualizar documento tecnico'
  }
};

export function getProjectStatusMeta(status: ProjectStatus): ProjectStatusMeta {
  return projectStatusMeta[status];
}
