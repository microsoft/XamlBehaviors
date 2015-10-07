#pragma once

#include "pch.h"
#include "SampleCommand.h"

using namespace Platform;
using namespace XAMLBehaviorsSampleCpp;

SampleCommand::SampleCommand(ExecuteDelegate^ execute, CanExecuteDelegate^ canExecute) : m_executeDelegate(execute), m_canExecuteDelegate(canExecute)
{
}

void SampleCommand::Execute(Object^ parameter)
{
	m_executeDelegate(parameter);
}

bool SampleCommand::CanExecute(Object^ parameter)
{
	return true;
}
