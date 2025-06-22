using DPAT1;
using DPAT1.Enums;
using DPAT1.Interfaces;
using DPAT1.States;
using DPAT1.Strategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DPAT1.Tests
{
    [TestClass]
    public class FSMTests
    {
        [TestMethod]
        public void InitialStateValidator_WhenInitialStateHasIncomingTransition_ShouldReturnFalse()
        {
            // Arrange
            var fsm = new FSM();
            var initialState = new InitialState { Id = "initial", Name = "Start", Type = StateType.INITIAL, Transitions = new List<Transition>() };
            var simpleState = new SimpleState { Id = "simple", Name = "Simple", Type = StateType.SIMPLE, Transitions = new List<Transition>() };

            fsm.AddState(initialState);
            fsm.AddState(simpleState);

            var transition = new Transition("t1", simpleState, initialState);
            fsm.AddTransition(transition);

            var validator = new InitialStateTransitionsValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InitialStateValidator_WhenNoInitialStatesExist_ShouldReturnTrue()
        {
            // Arrange
            var fsm = new FSM();
            var simpleState = new SimpleState { Id = "simple", Name = "Simple", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            fsm.AddState(simpleState);

            var validator = new InitialStateTransitionsValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonDeterministicValidator_WhenMultipleAutomaticTransitions_ShouldReturnFalse()
        {
            // Arrange
            var fsm = new FSM();
            var sourceState = new SimpleState { Id = "source", Name = "Source", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState1 = new SimpleState { Id = "target1", Name = "Target1", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState2 = new SimpleState { Id = "target2", Name = "Target2", Type = StateType.SIMPLE, Transitions = new List<Transition>() };

            fsm.AddState(sourceState);
            fsm.AddState(targetState1);
            fsm.AddState(targetState2);

            var automaticTransition1 = new Transition("t1", sourceState, targetState1, null, null);
            var automaticTransition2 = new Transition("t2", sourceState, targetState2, null, null);
            fsm.AddTransition(automaticTransition1);
            fsm.AddTransition(automaticTransition2);

            var validator = new NonDeterministicTransitionsValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NonDeterministicValidator_WhenSameTriggerWithUniqueGuards_ShouldReturnTrue()
        {
            // Arrange
            var fsm = new FSM();
            var sourceState = new SimpleState { Id = "source", Name = "Source", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState1 = new SimpleState { Id = "target1", Name = "Target1", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState2 = new SimpleState { Id = "target2", Name = "Target2", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var trigger = new Trigger { Id = "keypress", Description = "Key Press" };

            fsm.AddState(sourceState);
            fsm.AddState(targetState1);
            fsm.AddState(targetState2);
            fsm.AddTrigger(trigger);

            var transition1 = new Transition("t1", sourceState, targetState1, trigger, "key = a");
            var transition2 = new Transition("t2", sourceState, targetState2, trigger, "key = b");
            fsm.AddTransition(transition1);
            fsm.AddTransition(transition2);

            var validator = new NonDeterministicTransitionsValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonDeterministicValidator_WhenSameTriggerWithIdenticalGuards_ShouldReturnFalse()
        {
            // Arrange
            var fsm = new FSM();
            var sourceState = new SimpleState { Id = "source", Name = "Source", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState1 = new SimpleState { Id = "target1", Name = "Target1", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState2 = new SimpleState { Id = "target2", Name = "Target2", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var trigger = new Trigger { Id = "keypress", Description = "Key Press" };

            fsm.AddState(sourceState);
            fsm.AddState(targetState1);
            fsm.AddState(targetState2);
            fsm.AddTrigger(trigger);

            var transition1 = new Transition("t1", sourceState, targetState1, trigger, "key = a");
            var transition2 = new Transition("t2", sourceState, targetState2, trigger, "key = a");
            fsm.AddTransition(transition1);
            fsm.AddTransition(transition2);

            var validator = new NonDeterministicTransitionsValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UnreachableStateValidator_WhenAllStatesReachable_ShouldReturnTrue()
        {
            // Arrange
            var fsm = new FSM();
            var initialState = new InitialState { Id = "initial", Name = "Start", Type = StateType.INITIAL, Transitions = new List<Transition>() };
            var simpleState = new SimpleState { Id = "simple", Name = "Simple", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var finalState = new FinalState { Id = "final", Name = "End", Type = StateType.FINAL, Transitions = new List<Transition>() };

            fsm.AddState(initialState);
            fsm.AddState(simpleState);
            fsm.AddState(finalState);

            var transition1 = new Transition("t1", initialState, simpleState);
            var transition2 = new Transition("t2", simpleState, finalState);
            fsm.AddTransition(transition1);
            fsm.AddTransition(transition2);

            var validator = new UnreachableStateValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnreachableStateValidator_WhenNoInitialStateAndOneSimpleStateUnreachable_ShouldReturnTrue()
        {
            // Arrange
            var fsm = new FSM();
            var simpleState1 = new SimpleState { Id = "simple1", Name = "Simple1", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var simpleState2 = new SimpleState { Id = "simple2", Name = "Simple2", Type = StateType.SIMPLE, Transitions = new List<Transition>() };

            fsm.AddState(simpleState1);
            fsm.AddState(simpleState2);

            // Only one transition - simpleState1 becomes unreachable (implicit initial state)
            var transition = new Transition("t1", simpleState1, simpleState2);
            fsm.AddTransition(transition);

            var validator = new UnreachableStateValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnreachableStateValidator_WhenCompoundStateReachableViaChild_ShouldReturnTrue()
        {
            // Arrange
            var fsm = new FSM();
            var initialState = new InitialState { Id = "initial", Name = "Start", Type = StateType.INITIAL, Transitions = new List<Transition>() };
            var compoundState = new CompoundState { Id = "compound", Name = "Compound", Type = StateType.COMPOUND, Transitions = new List<Transition>(), Children = new List<IState>() };
            var childState = new SimpleState { Id = "child", Name = "Child", Type = StateType.SIMPLE, Transitions = new List<Transition>() };

            compoundState.AddChild(childState);

            fsm.AddState(initialState);
            fsm.AddState(compoundState);
            fsm.AddState(childState);

            var transition = new Transition("t1", initialState, childState);
            fsm.AddTransition(transition);

            var validator = new UnreachableStateValidatorStrategy();

            // Act
            bool result = validator.IsValid(fsm);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FSMBuilder_WhenAddingValidState_ShouldReturnStateWithCorrectProperties()
        {
            // Arrange
            var builder = new FSMBuilder();
            string id = "test_state";
            string parent = "_";
            string description = "Test State";
            string type = "SIMPLE";

            // Act
            var result = builder.AddState(id, parent, description, type);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(description, result.Name);
            Assert.AreEqual(StateType.SIMPLE, result.Type);
            Assert.IsInstanceOfType(result, typeof(SimpleState));
        }

        [TestMethod]
        public void Transition_WhenCreatedWithAllParameters_ShouldHaveCorrectProperties()
        {
            // Arrange
            var sourceState = new SimpleState { Id = "source", Name = "Source", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var targetState = new SimpleState { Id = "target", Name = "Target", Type = StateType.SIMPLE, Transitions = new List<Transition>() };
            var trigger = new Trigger { Id = "trigger1", Description = "Test Trigger" };
            var effect = new Action { Id = "effect1", Description = "Test Effect", Type = ActionType.TRANSITION_ACTION };
            string id = "t1";
            string guard = "condition = true";

            // Act
            var transition = new Transition(id, sourceState, targetState, trigger, guard, effect);

            // Assert
            Assert.AreEqual(id, transition.Id);
            Assert.AreEqual(sourceState, transition.Source);
            Assert.AreEqual(targetState, transition.Target);
            Assert.AreEqual(trigger, transition.Trigger);
            Assert.AreEqual(guard, transition.Guard);
            Assert.AreEqual(effect, transition.Effect);
        }
    }
}