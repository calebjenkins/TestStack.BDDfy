﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestStack.BDDfy.Samples
{
    public class AsyncExample
    {
        private Sut _sut;

        public async void GivenSomeAsyncSetup()
        {
            _sut = await CreateSut();
        }

        public void ThenBddfyHasWaitedForThatSetupToCompleteBeforeContinuing()
        {
            Assert.NotNull(_sut);
        }

        public async Task AndThenBddfyShouldCaptureExceptionsThrownInAsyncMethod()
        {
            await Task.Yield();
            throw new Exception("Exception in async void method!!");
        }

        private async Task<Sut> CreateSut()
        {
            await Task.Delay(500);
            return new Sut();
        }

        [Test]
        [Explicit]
        public void Run()
        {
            this.BDDfy();
        }

        internal class Sut
        {
        }
    }
}